using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProposalManagement.Application.Commands;

namespace ProposalManagement.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProposalController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<ProposalController> _logger;

    public ProposalController(ILogger<ProposalController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost(Name = "Create")]
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
}