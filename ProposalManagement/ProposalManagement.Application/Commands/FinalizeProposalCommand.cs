using System.Text.Json.Serialization;
using MediatR;
using ProposalManagement.Application.Core;
using ProposalManagement.Domain.Enums;
using ProposalManagement.Infrastructure.Shared;

namespace ProposalManagement.Application.Commands;

public class FinalizeProposalCommand : CreateCounterProposalCommand
{
    [JsonIgnore]
    public ProposalStatus Status { get; set; }
    
    
}