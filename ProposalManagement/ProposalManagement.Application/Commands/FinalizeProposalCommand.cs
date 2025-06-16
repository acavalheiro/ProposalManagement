using MediatR;
using ProposalManagement.Application.Core;
using ProposalManagement.Domain.Enums;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Commands;

public class FinalizeProposalCommand : BaseRequest, IRequest<Result<Guid?>>
{
    public ProposalStatus Status { get; set; }
    
    public CreateCounterProposalCommand? CreateCounterProposal { get; set; }
}