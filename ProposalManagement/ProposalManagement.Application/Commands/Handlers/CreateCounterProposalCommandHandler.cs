using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProposalManagement.Application.Core.Validators;
using ProposalManagement.Domain.Entities;
using ProposalManagement.Domain.Enums;
using ProposalManagement.Infrastructure.Data;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Commands.Handlers;

public class CreateCounterProposalCommandHandler : IRequestHandler<CreateCounterProposalCommand, Result<Guid>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<CreateProposalCommandHandler> _logger;
    private readonly CreateProposalValidator _createProposalValidator;

    public CreateCounterProposalCommandHandler(ApplicationDbContext applicationDbContext,
        ILogger<CreateProposalCommandHandler> logger, CreateProposalValidator createProposalValidator)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
        _createProposalValidator = createProposalValidator;
    }

    public async Task<Result<Guid>> Handle(CreateCounterProposalCommand request, CancellationToken cancellationToken)
    {
        var result = await _createProposalValidator.ValidateCounterProposalAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return result.Error!;

        

        var transaction = await _applicationDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var proposal = await _applicationDbContext.Proposals
                .FirstAsync(p => p.ProposalId == request.ParentProposalId, cancellationToken);
            
            proposal.UpdateStatus(ProposalStatus.Abandoned, request.AuthenticatedUserId);
            
            
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
}