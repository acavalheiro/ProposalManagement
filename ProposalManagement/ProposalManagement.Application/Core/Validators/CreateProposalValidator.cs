using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProposalManagement.Application.Commands;
using ProposalManagement.Application.Commands.Handlers;
using ProposalManagement.Domain.Entities;
using ProposalManagement.Domain.Enums;
using ProposalManagement.Infrastructure.Data;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Core.Validators;

public class CreateProposalValidator
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<CreateProposalCommandHandler> _logger;

    public CreateProposalValidator(ApplicationDbContext applicationDbContext, ILogger<CreateProposalCommandHandler> logger)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
    }
    
    public async Task<Result> ValidateCreateAsync(CreateProposalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await ValidateAsync(request, cancellationToken);
            
            if (!result.IsSuccess)
                return result;
            
            if (_applicationDbContext.Proposals.Any(p => p.ItemId == request.ItemId))
                return Error.Validation("ProposalAlreadyExists", "A proposal for this item already exists.");
            
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while validating the proposal creation request.");
            return Errors.CreateException(e.ToString());
        }
        
    }
    
    public async Task<Result> ValidateCounterProposalAsync(CreateCounterProposalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await ValidateAsync(request, cancellationToken);
            
            if (!result.IsSuccess)
                return result;
            
            if (string.IsNullOrEmpty(request.Information))
                return Error.Validation("InformationRequired", "Information is required for the counter proposal.");
            
            var parentProposal = await _applicationDbContext.Proposals
                .FirstOrDefaultAsync(p => p.ProposalId == request.ParentProposalId, cancellationToken);
            
            if (parentProposal == null)
                return Errors.NotFound("Proposal",request.ParentProposalId.ToString());
            
            if (parentProposal.CreatedById == request.AuthenticatedUserId)
                return Error.Validation("CannotCounterOwnProposal", "You cannot create a counter proposal for your own proposal.");
            
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while validating the proposal creation request.");
            return Errors.CreateException(e.ToString());
        }
    }
    
    private async Task<Result> ValidateAsync(CreateProposalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.UserId ==request.AuthenticatedUserId, cancellationToken);
            if (user == null)
                return Errors.NotFound(nameof(User),request.AuthenticatedUserId.ToString());
        
            var item = await _applicationDbContext.Items.FirstOrDefaultAsync(i => i.ItemId == request.ItemId, cancellationToken);
            if (item == null)
                return Errors.NotFound(nameof(Item),request.ItemId.ToString());
        
            if (item.PartyId != user.PartyId)
                return Error.Validation("ItemNotBelongToParty", "The item does not belong to the party of the authenticated user.");
        
            
        
            if (request.AllocationQuantity < 1)
                return Error.Validation("InvalidAllocationQuantity", "Allocation quantity must be greater than 0.");
        
            if (request is { AllocationType: ProposalAllocationType.Percentage, AllocationQuantity: > 100 })
                return Error.Validation("InvalidAllocationPercentage", "Allocation percentage must be between 0 and 100.");
            
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while validating the proposal creation request.");
            return Errors.CreateException(e.ToString());
        }
    }
}