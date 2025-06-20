using Microsoft.Extensions.DependencyInjection;
using ProposalManagement.Application.Commands.Handlers;
using ProposalManagement.Application.Core.Validators;
using ProposalManagement.Application.Validators;

namespace ProposalManagement.Application;

public static class ConfigurationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateProposalCommandHandler>());
        services.AddScoped<ICreateProposalValidator, CreateProposalValidator>();
        services.AddScoped<IFinalizeProposalValidator, FinalizeProposalValidator>();
        return services;
    }
}