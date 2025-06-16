using Microsoft.EntityFrameworkCore;
using ProposalManagement.Domain.Entities;

namespace ProposalManagement.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() : base()
    {
        
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Party> Parties { get; set; }
    public virtual DbSet<Item> Items { get; set; }
    
    public virtual DbSet<Proposal> Proposals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User")
            .HasOne(e => e.Party)
            .WithMany(e => e.Users)
            .HasForeignKey("PartyId");
        
        modelBuilder.Entity<Item>()
            .ToTable("Item")
            .HasMany(e => e.Parties)
            .WithMany(e => e.Items)
            .UsingEntity(
                "ItemParty",
                r => r.HasOne(typeof(Party)).WithMany().HasForeignKey("PartyId").HasPrincipalKey(nameof(Party.PartyId)),
                l => l.HasOne(typeof(Item)).WithMany().HasForeignKey("ItemId").HasPrincipalKey(nameof(Item.ItemId)),
                j => j.HasKey("ItemId", "PartyId"));;
        
        modelBuilder.Entity<Party>()
            .ToTable("Party");

        
        
        modelBuilder.Entity<Proposal>().ToTable("Proposal");
    }
}