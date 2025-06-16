using MediatR;
using ProposalManagement.Application.Core;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Queries;

public class GetItemProposalsInformationQuery : BaseRequest, IRequest<Result<ItemProposals>>
{
    public Guid ItemId { get; set; }
}

public record ItemProposals
{
    public Guid Id { get; init; }
     public string Name { get; init; }
     public List<ItemProposalsData> Proposals { get; init; }
    
}

public record ItemProposalsData
{
    public Guid ProposalId { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string CreatedBy { get; set; }
    public string Information { get; set; }
    public string? CompletedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}