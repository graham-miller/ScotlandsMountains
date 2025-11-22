using Microsoft.AspNetCore.Mvc;
using ScotlandsMountains.Application.Adapters;
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

    [HttpGet("{id:int}", Name = nameof(Get))]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
    {
        var query = new GetDobihFileInfoQuery(id);
        var result = await _mediator.SendAsync(query, cancellationToken);

        if (result.IsFailure) return BadRequest();
        else if (result.Value is null) return NotFound();

        return Ok(new DobihFileModel(result.Value));
    }

    [HttpPost(Name = nameof(Upload))]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0) return BadRequest();

        var command = new UploadDobihFileCommand(file.OpenReadStream());

        var result = await _mediator.SendAsync(command, cancellationToken);

        if (result.IsFailure) return BadRequest(result.ErrorMessage);

        return AcceptedAtRoute(
            routeName: nameof(Get),
            routeValues: new { id = result.Value.Id },
            value: new DobihFileModel(result.Value));
    }
}
