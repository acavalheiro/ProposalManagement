#nullable enable
using ProposalManagement.Domain.Entities;

namespace ProposalManagement.Domain.Core.Entities;

public abstract class Entity : IEntity
{
    public Entity()
    {
        CreatedDate = DateTime.UtcNow;
    }
    public Guid CreatedById { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? ModifiedById { get; set; }
    public DateTime? ModifiedDate { get; set; }
    
    public User CreatedBy { get; set; } = null!;
    public User? ModifiedBy { get; set; }
}

public interface IEntity
{
    public Guid CreatedById { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? ModifiedById { get; set; }
    public DateTime? ModifiedDate { get; set; }
}