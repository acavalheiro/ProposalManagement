using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProposalManagement.Domain.Entities;
using ProposalManagement.Infrastructure.Data;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Core.Validators;

public abstract class BaseValidator : IBaseValidator
{
    public readonly ApplicationDbContext ApplicationDbContext;
    private readonly ILogger _logger;

    protected BaseValidator(ApplicationDbContext applicationDbContext, ILogger logger)
    {
        ApplicationDbContext = applicationDbContext;
        _logger = logger;
    }
    
    public  async Task<Result<User>> ValidateAuthenticatedUser(Guid userId, CancellationToken cancellationToken)
    {
        var user = await ApplicationDbContext.Users.FirstOrDefaultAsync(u => u.UserId ==userId, cancellationToken);
        if (user == null)
            return Errors.NotFound(nameof(User),userId.ToString());
        

        return user;
    }
}

public interface IBaseValidator
{
    Task<Result<User>> ValidateAuthenticatedUser(Guid userId, CancellationToken cancellationToken);
}