using MediatR;
using Microsoft.Extensions.Logging;
using ProposalManagement.Domain.Entities;
using ProposalManagement.Infrastructure.Data;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Commands.Handlers;

public class CreateProposalCommandHandler : IRequestHandler<CreateProposalCommand, Result>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<CreateProposalCommandHandler> _logger;

    public CreateProposalCommandHandler(ApplicationDbContext applicationDbContext, ILogger<CreateProposalCommandHandler> logger)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(CreateProposalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //validate if request.AuthenticatedUserId valid user
            var user = await _applicationDbContext.Users.FindAsync([request.AuthenticatedUserId], cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User with Id: {UserId} not found", request.AuthenticatedUserId);
                return Result.Failure(Errors.NotFound(request.AuthenticatedUserId.ToString()));
            }
            
            var item = await _applicationDbContext.Items.FindAsync([request.ItemId], cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Item with Id: {ItemId} not found", request.ItemId);
                return Result.Failure(Errors.NotFound(request.ItemId.ToString()));
            }
            
            if (item.PartyId != user.PartyId)
            {
                _logger.LogWarning("Item with Id: {ItemId} does not belong to the party of User with Id: {UserId}", request.ItemId, request.AuthenticatedUserId);
                return Result.Failure(Error.Validation("ItemNotBelongToParty", "The item does not belong to the party of the authenticated user."));
            }
            
            

            var proposal = new Proposal
            {
                ItemId = request.ItemId,
                Information = request.Information,
                ProposalAllocationTypeid = request.AllocationType,
                CreatedById = request.AuthenticatedUserId,
                
            };

            await _applicationDbContext.Proposals.AddAsync(proposal, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
            
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while creating a proposal.");
            return Result.Failure(Errors.CreateException(e.ToString()));
        }
        
        
    }
}