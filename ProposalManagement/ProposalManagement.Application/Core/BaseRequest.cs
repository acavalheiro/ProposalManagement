namespace ProposalManagement.Application.Core;

public abstract class BaseRequest
{
    public Guid AuthenticatedUserId { get; set; }
}