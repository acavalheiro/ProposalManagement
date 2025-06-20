using ProposalManagement.Application.Commands;
using ProposalManagement.Domain.Entities;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Core.Validators;

public interface IFinalizeProposalValidator : IBaseValidator
{
    Task<Result<Proposal>> ValidateFinalizeProposalAsync(FinalizeProposalCommand request,
        CancellationToken cancellationToken);
}