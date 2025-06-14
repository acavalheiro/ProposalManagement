using Microsoft.EntityFrameworkCore;
using ProposalManagement.Domain.Entities;

namespace ProposalManagement.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Party> Parties { get; set; }
    public DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Party>().ToTable("Party");
        modelBuilder.Entity<Item>().ToTable("Item");
        
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
    }
}