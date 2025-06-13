using ProposalManagement.Domain.Core.Entities;

namespace ProposalManagement.Domain.Entities;

public class Item : Entity , IEntity
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public ItemStatus ItemStatusId { get; set; }
    public Guid PartyId { get; set; }
    public Party Party { get; set; } = null!;
    
}

public enum ItemStatus
{
    Private = 1,
    Shared = 2,
    Retired = 3
}