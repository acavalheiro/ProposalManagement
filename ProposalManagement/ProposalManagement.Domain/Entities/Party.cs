namespace ProposalManagement.Domain.Entities;

public class Party
{
    public int PartyId { get; set; }
    public string Name { get; set; }
    
    public ICollection<User> Users { get; } = new List<User>(); 
    public ICollection<Item> Items { get; } = new List<Item>(); 
}