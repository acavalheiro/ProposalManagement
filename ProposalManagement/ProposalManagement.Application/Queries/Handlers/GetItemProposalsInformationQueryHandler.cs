using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProposalManagement.Application.Mappers;
using ProposalManagement.Domain.Entities;
using ProposalManagement.Infrastructure.Data;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Queries.Handlers;

public class GetItemProposalsInformationQueryHandler : IRequestHandler<GetItemProposalsInformationQuery, Result<ItemProposals>>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<GetItemProposalsInformationQueryHandler> _logger;

    public GetItemProposalsInformationQueryHandler(ApplicationDbContext applicationDbContext, ILogger<GetItemProposalsInformationQueryHandler> logger)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
    }

    public async Task<Result<ItemProposals>> Handle(GetItemProposalsInformationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.UserId ==request.AuthenticatedUserId, cancellationToken);
            if (user == null)
                return Errors.NotFound(nameof(User),request.AuthenticatedUserId.ToString());
            
            var item = await _applicationDbContext.Items.AsNoTracking()
                .FirstOrDefaultAsync(i => i.ItemId == request.ItemId, cancellationToken);
            
            

            if (item == null)
                return Error.NotFound("ItemNotFound", "The specified item was not found.");
            
            var proposals = await _applicationDbContext.Proposals.AsNoTracking()
                .Include(u => u.CreatedBy)
                .ThenInclude(u => u.Party)
                .Include(u => u.ModifiedBy)
                .Where(p => p.ItemId == request.ItemId)
                .ToListAsync(cancellationToken);


            return item.ToItemProposals(proposals,user.PartyId);


        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving item proposals information for ItemId: {ItemId}", request.ItemId);
            return Errors.CreateException(e.ToString());
        }
    }
}