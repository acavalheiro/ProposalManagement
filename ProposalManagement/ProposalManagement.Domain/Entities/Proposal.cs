using ProposalManagement.Domain.Core.Entities;
using ProposalManagement.Domain.Enums;

namespace ProposalManagement.Domain.Entities;

public class Proposal : Entity , IEntity
{
    
    public Guid ProposalId { get; set; }
    public Guid? ParentProposalId { get; set; }
    public Proposal ParentProposal { get; set; }
    public string Information { get; set; }
    public Guid ItemId { get; set; }
    public Item Item { get; set; }
    
    public ProposalAllocationType ProposalAllocationTypeid { get; set; }
    public ProposalStatus ProposalStatusId { get; set; } = ProposalStatus.New;
    public ProposalType ProposalTypeId { get; set; } = ProposalType.Initial;

}