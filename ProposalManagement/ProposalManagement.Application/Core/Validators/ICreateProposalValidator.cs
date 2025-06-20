using ProposalManagement.Application.Commands;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Core.Validators;

public interface ICreateProposalValidator : IBaseValidator
{
    Task<Result> ValidateCreateAsync(CreateProposalCommand request, CancellationToken cancellationToken);

    Task<Result> ValidateCounterProposalAsync(CreateCounterProposalCommand request,
        CancellationToken cancellationToken);
}