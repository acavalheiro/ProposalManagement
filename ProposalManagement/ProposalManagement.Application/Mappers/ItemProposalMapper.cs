using ProposalManagement.Application.Queries;
using ProposalManagement.Domain.Entities;
using ProposalManagement.Domain.Enums;

namespace ProposalManagement.Application.Mappers;

public static class ItemProposalMapper
{
    private const string ApprovedBySelf = "Proposal by {0} {1} on behalf of {2}";
    private const string ApprovedByOther = "Proposal by {0}";
    
    private const string CompletedBySelf = "{3} by {0} {1} on behalf of {2}";
    private const string CompletedByOther = "{1} by {0}";
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
                CompletedBy = GetCompletedBy(p.ModifiedBy,authenticatedUserPartyId, p.ProposalStatusId)
            }).ToList()
            
            
        };
    }

    private static string GetCreatedBy(User createdBy, Guid authenticatedUserPartyId)
    {
        return createdBy.PartyId == authenticatedUserPartyId ? 
            string.Format(ApprovedBySelf, createdBy.FirstName, createdBy.LastName, createdBy.Party.Name) : 
            string.Format(ApprovedByOther, createdBy.Party.Name);
    }
    
    private static string GetCompletedBy(User? modifiedBy, Guid authenticatedUserPartyId,
        ProposalStatus proposalStatusId)
    {
        if (modifiedBy == null)
            return string.Empty;
        
        return modifiedBy.PartyId == authenticatedUserPartyId ? string.Format(CompletedBySelf, modifiedBy.FirstName, modifiedBy.LastName, modifiedBy.Party.Name, proposalStatusId ) : string.Format(CompletedByOther, modifiedBy.Party.Name, proposalStatusId);
    }
}