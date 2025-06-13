namespace ProposalManagement.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public int PartyId { get; set; } 
    public Party Party { get; set; } = null!;
}