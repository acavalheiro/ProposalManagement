using Microsoft.EntityFrameworkCore;
using ProposalManagement.Domain.Entities;

namespace ProposalManagement.Infrastructure.Data;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Party> Parties { get; set; }
    public DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");
        
        modelBuilder.Entity<Party>().ToTable("Party");
        
        modelBuilder.Entity<Item>().ToTable("Item");
        
        
    }
}