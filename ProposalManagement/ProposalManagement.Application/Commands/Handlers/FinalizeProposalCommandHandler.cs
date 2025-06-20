using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProposalManagement.Application.Core.Validators;
using ProposalManagement.Domain.Entities;
using ProposalManagement.Domain.Enums;
using ProposalManagement.Infrastructure.Data;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Commands.Handlers;

public class FinalizeProposalCommandHandler : IRequestHandler<FinalizeProposalCommand, Result<Guid>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<CreateProposalCommandHandler> _logger;
    private readonly IFinalizeProposalValidator _finalizeProposalValidator;
    private readonly ICreateProposalValidator _createProposalValidator;

    public FinalizeProposalCommandHandler(IFinalizeProposalValidator finalizeProposalValidator, ILogger<CreateProposalCommandHandler> logger, ApplicationDbContext applicationDbContext, ICreateProposalValidator createProposalValidator)
    {
        _finalizeProposalValidator = finalizeProposalValidator;
        _logger = logger;
        _applicationDbContext = applicationDbContext;
        _createProposalValidator = createProposalValidator;
    }

    public async Task<Result<Guid>> Handle(FinalizeProposalCommand request, CancellationToken cancellationToken)
    {
        var result = await _finalizeProposalValidator.ValidateAuthenticatedUser(request.AuthenticatedUserId, cancellationToken);

        if (!result.IsSuccess)
            return result.Error!;
        
        var parentProposalResult = await _finalizeProposalValidator.ValidateFinalizeProposalAsync(request, cancellationToken);
        
        if (!parentProposalResult.IsSuccess)
            return parentProposalResult.Error!;

        if (request.Status == ProposalStatus.Rejected)
        {
            var createCounterProposalValidationResult = await _createProposalValidator.ValidateCounterProposalAsync(request, cancellationToken);
            if (!createCounterProposalValidationResult.IsSuccess)
                return createCounterProposalValidationResult.Error!;
        }
        
        var transaction = await _applicationDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var proposal = await _applicationDbContext.Proposals
                .FirstAsync(p => p.ProposalId == request.ParentProposalId, cancellationToken);
            
            proposal.UpdateStatus(request.Status, request.AuthenticatedUserId);
            
            if (request.Status == ProposalStatus.Rejected)
                await this.CreateCounterProposalAsync(request, proposal, cancellationToken);
            
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);

            return proposal.ProposalId;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(e.Message);
            return Errors.CreateException(e.ToString());
        }
    }

    private async Task CreateCounterProposalAsync(FinalizeProposalCommand request, Proposal proposal, CancellationToken cancellationToken)
    {
        var counterProposal = new Proposal
        {
            ItemId = proposal.ItemId,
            Information = request.Information,
            ProposalAllocationTypeid = request.AllocationType,
            ParentProposalId = request.ParentProposalId,
            ProposalTypeId = ProposalType.Counter,
            CreatedById = request.AuthenticatedUserId,
        };

        await _applicationDbContext.Proposals.AddAsync(counterProposal, cancellationToken);
    }
}