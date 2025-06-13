namespace ProposalManagement.Domain.Entities;

public class Party
{
    public int PartyId { get; set; }
    public string Name { get; set; }
    
    public ICollection<User> Posts { get; } = new List<User>(); 
}