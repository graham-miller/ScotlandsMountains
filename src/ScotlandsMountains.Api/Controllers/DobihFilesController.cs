using Microsoft.AspNetCore.Mvc;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.UseCases.DobihFiles;

namespace ScotlandsMountains.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DobihFilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DobihFilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}", Name = "GetDobihFile")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
    {
        var query = new GetDobihFileStatusQuery(id);
        var result = await _mediator.SendAsync(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.ErrorMessage);
        }
        else if (result.Value is null)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0) return BadRequest();

        var command = new UploadDobihFileCommand(file.OpenReadStream());

        var result = await _mediator.SendAsync(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.ErrorMessage);
        }

        return AcceptedAtRoute(
            routeName: "GetDobihFile",
            routeValues: new { id = result.Value.Id },
            value: result.Value);
    }
}
