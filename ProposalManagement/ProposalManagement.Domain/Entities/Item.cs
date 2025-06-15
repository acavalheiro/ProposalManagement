using ProposalManagement.Domain.Core.Entities;
using ProposalManagement.Domain.Enums;

namespace ProposalManagement.Domain.Entities;

public class Item : Entity , IEntity
{
    public Guid ItemId { get; set; }
    public string Name { get; set; }
    public ItemStatus ItemStatusId { get; set; }
    public Guid PartyId { get; set; }
    public Party Party { get; set; } = null!;
    
}