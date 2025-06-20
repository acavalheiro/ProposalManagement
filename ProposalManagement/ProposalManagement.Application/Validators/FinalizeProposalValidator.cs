using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProposalManagement.Application.Commands;
using ProposalManagement.Application.Core.Validators;
using ProposalManagement.Domain.Entities;
using ProposalManagement.Domain.Enums;
using ProposalManagement.Infrastructure.Data;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Validators;

public class FinalizeProposalValidator : BaseValidator , IFinalizeProposalValidator
{
    private readonly ILogger<FinalizeProposalValidator> _logger;
    public FinalizeProposalValidator(ApplicationDbContext applicationDbContext, ILogger<FinalizeProposalValidator> logger) : base(applicationDbContext, logger)
    {
        _logger = logger;
    }

    public async Task<Result<Proposal>> ValidateFinalizeProposalAsync(FinalizeProposalCommand request,
        CancellationToken cancellationToken)
    {
        var parentProposal = await this.ApplicationDbContext.Proposals
            .FirstOrDefaultAsync(p => p.ProposalId == request.ParentProposalId, cancellationToken);
            
        if (parentProposal == null)
            return Errors.NotFound("Proposal",request.ParentProposalId.ToString());
            
        if(parentProposal.ProposalStatusId is   ProposalStatus.Abandoned or ProposalStatus.Approved or ProposalStatus.Rejected)
            return Error.Validation("InvalidParentProposalStatus", "The parent proposal cannot be countered.");
        
        if (request.Status is not (ProposalStatus.Approved or ProposalStatus.Rejected))
            return Error.Validation("InvalidStatus", "The status must be either Approved or Rejected.");

        return parentProposal;
    }
    
}