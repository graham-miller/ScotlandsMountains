using Microsoft.AspNetCore.Mvc;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.UseCases.Mountains;

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

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest();

        var command = new UploadFileCommand(file.OpenReadStream());

        await _mediator.SendAsync(command);

        return Accepted();
    }
}
