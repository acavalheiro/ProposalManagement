

using MediatR;
using ProposalManagement.Application.Core;
using ProposalManagement.Domain.Enums;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Commands;

public class CreateProposalCommand : BaseRequest, IRequest<Result>
{
    public Guid ItemId { get; set; }
    public string? Information { get; set; }
    public ProposalAllocationType AllocationType { get; set; }
    
}