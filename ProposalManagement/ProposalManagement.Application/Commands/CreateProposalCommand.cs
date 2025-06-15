

using MediatR;
using ProposalManagement.Application.Core;
using ProposalManagement.Domain.Enums;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Commands;

public class CreateProposalCommand : BaseRequest, IRequest<Result<Guid>>
{
    public Guid ItemId { get; set; }
    public string? Information { get; set; }
    public ProposalAllocationType AllocationType { get; set; }
    
    public int AllocationQuantity { get; set; }
    
}

public class CreateCounterProposalCommand : CreateProposalCommand, IRequest<Result<Guid>>
{
    public Guid ParentProposalId { get; set; }
    
}