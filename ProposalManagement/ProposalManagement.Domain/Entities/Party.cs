namespace ProposalManagement.Domain.Entities;

public class Party
{
    public Guid PartyId { get; set; }
    public string Name { get; set; }
    
    public ICollection<User> Users { get; } = new List<User>(); 
    
    public List<Item> Items { get; } = [];
}