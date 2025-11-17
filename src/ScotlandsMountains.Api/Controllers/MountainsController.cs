using Microsoft.AspNetCore.Mvc;
using ScotlandsMountains.Application.Dtos;
using ScotlandsMountains.Application.RequestMediator;
using ScotlandsMountains.Application.UseCases.Mountains;

namespace ScotlandsMountains.Api.Controllers;

[Route("[controller]")]
public class MountainsController : Controller
{
    private IMediator _mediator;

    public MountainsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var command = new CreateMountainCommand("Ben Nevis");

        var result = await _mediator.SendAsync(command, cancellationToken);

        return NoContent();
    }
}
