using ScotlandsMountains.Application.Dtos;
using ScotlandsMountains.Application.Ports;

namespace ScotlandsMountains.Application.UseCases.Mountains;

public record UploadFileCommand(Stream Content) : IRequest<Result>;

internal class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, Result>
{
    private readonly IFileStorageService _fileStorageService;

    public UploadFileCommandHandler(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public async Task<Result> HandleAsync(UploadFileCommand request, CancellationToken cancellationToken = default)
    {
        const string containerName = "dobih-files";
        var fileName = Guid.NewGuid().ToString();

        await _fileStorageService.UploadFileAsync(containerName, fileName, request.Content, cancellationToken);

        return Result.Success();
    }
}
