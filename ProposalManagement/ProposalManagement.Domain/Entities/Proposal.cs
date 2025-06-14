using ProposalManagement.Domain.Core.Entities;

namespace ProposalManagement.Domain.Entities;

public class Proposal : Entity , IEntity
{
    public Guid ProposalId { get; set; }
    public Guid? ParentProposalId { get; set; }
    public Proposal ParentProposal { get; set; }
    public string Information { get; set; }
    public Guid ItemId { get; set; }
    public Item Item { get; set; }
    
}