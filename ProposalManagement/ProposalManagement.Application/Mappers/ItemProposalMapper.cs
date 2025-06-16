using ProposalManagement.Application.Queries;
using ProposalManagement.Domain.Entities;

namespace ProposalManagement.Application.Mappers;

public static class ItemProposalMapper
{
    private const string ApprovedBySelf = "Proposal by {0} {1} on behalf of {2}";
    private const string ApprovedByOther = "Proposal by {0}";
    public static ItemProposals ToItemProposals(this Item item, List<Proposal> proposals, Guid authenticatedUserPartyId)
    {
        return new ItemProposals
        {
            Id = item.ItemId,
            Name = item.Name,
            Proposals = proposals.OrderBy(x => x.CreatedDate).Select(p => new ItemProposalsData
            {
                ProposalId = p.ProposalId,
                Status = p.ProposalStatusId.ToString(),
                Type = p.ProposalTypeId.ToString(),
                CreatedBy = GetCreatedBy(p.CreatedBy, authenticatedUserPartyId),
                CreatedDate = p.CreatedDate,
                Information = p.Information,
                //CompletedBy = p.ModifiedBy?.UserName ?? (p.ProposalStatusId == ProposalStatus.Completed ? string.Format(ApprovedBySelf, p.CreatedBy?.UserName, p.Item.Parties.FirstOrDefault()?.PartyName) : null)
            }).ToList()
            
            
        };
    }

    private static string GetCreatedBy(User createdBy, Guid authenticatedUserPartyId)
    {
        if (createdBy.PartyId == authenticatedUserPartyId)
            return string.Format(ApprovedBySelf, createdBy.FirstName, createdBy.LastName, createdBy.Party.Name);
        
        return string.Format(ApprovedByOther, createdBy.Party.Name);
    }
}