using ScotlandsMountains.Application.Adapters;
using ScotlandsMountains.Application.Ports;

namespace ScotlandsMountains.Application.UseCases.Mountains;

public record CreateMountainCommand(string Name) : IRequest<Result<MountainDto>>;

internal class CreateMountainCommandHandler : IRequestHandler<CreateMountainCommand, Result<MountainDto>>
{
    public Task<Result<MountainDto>> HandleAsync(CreateMountainCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
