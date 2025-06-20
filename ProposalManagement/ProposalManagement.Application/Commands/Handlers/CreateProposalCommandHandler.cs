using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProposalManagement.Application.Core.Validators;
using ProposalManagement.Domain.Entities;
using ProposalManagement.Domain.Enums;
using ProposalManagement.Infrastructure.Data;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Commands.Handlers;

public class CreateProposalCommandHandler : IRequestHandler<CreateProposalCommand, Result<Guid>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<CreateProposalCommandHandler> _logger;
    private readonly ICreateProposalValidator _createProposalValidator;

    public CreateProposalCommandHandler(ApplicationDbContext applicationDbContext, ILogger<CreateProposalCommandHandler> logger, ICreateProposalValidator createProposalValidator)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
        _createProposalValidator = createProposalValidator;
    }

    public async Task<Result<Guid>> Handle(CreateProposalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _createProposalValidator.ValidateCreateAsync(request,cancellationToken);

            if (!result.IsSuccess)
                return result.Error!;
            

            var proposal = new Proposal
            {
                ItemId = request.ItemId,
                Information = request.Information,
                ProposalAllocationTypeid = request.AllocationType,
                CreatedById = request.AuthenticatedUserId,
                
                
            };

            await _applicationDbContext.Proposals.AddAsync(proposal, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return proposal.ProposalId;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while creating a proposal.");
            return Errors.CreateException(e.ToString());
        }
        
        
    }
    
    
}