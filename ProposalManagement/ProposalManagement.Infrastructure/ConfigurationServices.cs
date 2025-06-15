using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProposalManagement.Infrastructure.Data;

namespace ProposalManagement.Infrastructure;

public static class ConfigurationServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Configure the DbContext with the provided connection string
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        return services;
    }
}