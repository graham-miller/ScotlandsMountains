using ScotlandsMountains.Application.Dtos;
using ScotlandsMountains.Application.RequestMediator;

namespace ScotlandsMountains.Application.UseCases.Mountains;

public record CreateMountainCommand(string Name) : IRequest<Result<MountainDto>>;

internal class CreateMountainHandler : IRequestHandler<CreateMountainCommand, Result<MountainDto>>
{
    public Task<Result<MountainDto>> HandleAsync(CreateMountainCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
