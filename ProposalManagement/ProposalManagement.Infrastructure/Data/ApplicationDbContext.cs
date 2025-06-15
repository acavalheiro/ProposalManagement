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
    
    public DbSet<Proposal> Proposals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");
        
        modelBuilder.Entity<Item>().ToTable("Item");
        
        modelBuilder.Entity<Party>().ToTable("Party");
        modelBuilder.Entity<Party>()
            .HasMany(e => e.Users)
            .WithOne(e => e.Party)
            .HasForeignKey(e => e.UserId)
            .IsRequired();
        
        modelBuilder.Entity<Party>()
            .HasMany(e => e.Items)
            .WithOne(e => e.Party)
            .HasForeignKey(e => e.PartyId)
            .IsRequired();
        
        modelBuilder.Entity<Proposal>().ToTable("Proposal");
    }
}