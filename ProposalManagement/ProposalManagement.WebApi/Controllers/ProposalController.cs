using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProposalManagement.Application.Commands;
using ProposalManagement.Application.Queries;
using ProposalManagement.Domain.Enums;

namespace ProposalManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProposalController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<ProposalController> _logger;

    public ProposalController(ILogger<ProposalController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProposalCommand command)
    {
        try
        {
            
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(Create), new { id = result.Value }, result.Value);
            }

            _logger.LogError("Error creating proposal: {Error}", result.Error);
            return BadRequest(result.Error);

        }
        catch (Exception e)
        {
            return Problem(
                detail: e.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unexpected error occurred while creating the proposal."
            );
        }
    }
    
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id:guid}/CounterProposal")]
    public async Task<IActionResult> CounterProposal(Guid id, [FromBody] CreateCounterProposalCommand command)
    {
        try
        {
            command.ParentProposalId = id;
            var result = await _mediator.Send(command);
    
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(Create), new { id = result.Value }, result.Value);
            }
    
            _logger.LogError("Error creating counter proposal: {Error}", result.Error);
            return BadRequest(result.Error);
    
        }
        catch (Exception e)
        {
            return Problem(
                detail: e.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unexpected error occurred while creating the proposal."
            );
        }
    }
    
    [HttpGet("GetByItem")]
    public async Task<IActionResult> GetByItem([FromQuery]GetItemProposalsInformationQuery command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result.Value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred during the test.");
            return Problem(
                detail: e.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unexpected error occurred during the test."
            );
        }
    }
    
    [HttpPut("{id:guid}/Approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Approve(Guid id,[FromBody] FinalizeProposalCommand command)
    {
        try
        {
            command.ParentProposalId = id;
            command.Status = ProposalStatus.Approved;
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok();
            }

            _logger.LogError("Error creating proposal: {Error}", result.Error);
            return BadRequest(result.Error);

        }
        catch (Exception e)
        {
            return Problem(
                detail: e.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unexpected error occurred while approving the proposal."
            );
        }
    }
    
    [HttpPost("{id:guid}/Reject")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Reject(Guid id,[FromBody] FinalizeProposalCommand command)
    {
        try
        {
            command.ParentProposalId = id;
            command.Status = ProposalStatus.Rejected;
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(Create), new { id = result.Value }, result.Value);
            }

            _logger.LogError("Error creating proposal: {Error}", result.Error);
            return BadRequest(result.Error);

        }
        catch (Exception e)
        {
            return Problem(
                detail: e.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unexpected error occurred while reject the proposal."
            );
        }
    }
}