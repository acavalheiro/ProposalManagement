using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProposalManagement.Application.Commands;
using ProposalManagement.Application.Commands.Handlers;
using ProposalManagement.Application.Core.Validators;
using ProposalManagement.Domain.Entities;
using ProposalManagement.Domain.Enums;
using ProposalManagement.Infrastructure.Data;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Validators;

public class CreateProposalValidator :  BaseValidator,  ICreateProposalValidator
{
    private readonly ILogger<CreateProposalCommandHandler> _logger;

    public CreateProposalValidator(ApplicationDbContext applicationDbContext, ILogger<CreateProposalCommandHandler> logger) : base(applicationDbContext,logger)
    {

        _logger = logger;
    }
    
    public async Task<Result> ValidateCreateAsync(CreateProposalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await ValidateAsync(request, cancellationToken);
            
            if (!result.IsSuccess)
                return result;
            
            if (this.ApplicationDbContext.Proposals.Any(p => p.ItemId == request.ItemId))
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
            var userResult = await ValidateAuthenticatedUser(request.AuthenticatedUserId, cancellationToken);
            if (!userResult.IsSuccess)
                return userResult;
            
            if (string.IsNullOrEmpty(request.Information))
                return Error.Validation("InformationRequired", "Information is required for the counter proposal.");
            
            var parentProposal = await this.ApplicationDbContext.Proposals
                .FirstOrDefaultAsync(p => p.ProposalId == request.ParentProposalId, cancellationToken);
            
            if (parentProposal == null)
                return Errors.NotFound("Proposal",request.ParentProposalId.ToString());
            
            if(parentProposal.ProposalStatusId is   ProposalStatus.Abandoned or ProposalStatus.Approved or ProposalStatus.Rejected)
                return Error.Validation("InvalidParentProposalStatus", "The parent proposal cannot be countered.");
            
            var sharedFieldsResult = await ValidateSharedFields(request.AllocationType, request.AllocationQuantity);
            if (!sharedFieldsResult.IsSuccess)
                return sharedFieldsResult;
            
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
            var userResult = await ValidateAuthenticatedUser(request.AuthenticatedUserId, cancellationToken);
            if (!userResult.IsSuccess)
                return userResult;
               
            var user = userResult.Value;
        
            var item = await this.ApplicationDbContext.Items.Include(item => item.Parties).FirstOrDefaultAsync(i => i.ItemId == request.ItemId, cancellationToken);
            if (item == null)
                return Errors.NotFound(nameof(Item),request.ItemId.ToString());
        
            if (item.Parties.All(ip => ip.PartyId != user.PartyId))
                return Error.Validation("ItemNotBelongToParty", "The item does not belong to the party of the authenticated user.");
            
            var sharedFieldsResult = await ValidateSharedFields(request.AllocationType, request.AllocationQuantity);
            if (!sharedFieldsResult.IsSuccess)
                return sharedFieldsResult;
            
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while validating the proposal creation request.");
            return Errors.CreateException(e.ToString());
        }
    }
    

    private Task<Result> ValidateSharedFields(ProposalAllocationType proposalAllocationType, int allocationQuantity)
    {
        if (proposalAllocationType == ProposalAllocationType.Amount && allocationQuantity <= 0 )
            return Task.FromResult<Result>(Error.Validation("InvalidAllocationQuantity", "Allocation quantity must be greater than 0."));
        
        if (proposalAllocationType ==   ProposalAllocationType.Percentage && (allocationQuantity > 100 || allocationQuantity <0 ))
            return Task.FromResult<Result>(Error.Validation("InvalidAllocationPercentage", "Allocation percentage must be between 0 and 100."));
            
        return Task.FromResult(Result.Success());
    }
}