using ScotlandsMountains.Application.Dispatcher;
using ScotlandsMountains.Application.Dtos;

namespace ScotlandsMountains.Application.UseCases.Mountains;

internal record CreateMountainCommand(string Name);

internal class CreateMountain : Handler<CreateMountainCommand, Result<MountainDto>>
{
    public override Result<MountainDto> Handle(CreateMountainCommand request)
    {
        throw new NotImplementedException();
    }
}
