using System.Text.Json.Serialization;
using MediatR;
using ProposalManagement.Application.Core;
using ProposalManagement.Domain.Enums;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Commands;

public class CreateCounterProposalCommand : BaseRequest, IRequest<Result<Guid>>
{
    [JsonIgnore]
    public Guid ParentProposalId { get; set; }
    public string? Information { get; set; }
    public ProposalAllocationType AllocationType { get; set; }
    
    public int AllocationQuantity { get; set; }
}