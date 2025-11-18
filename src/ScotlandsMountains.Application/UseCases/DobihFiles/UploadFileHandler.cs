using ScotlandsMountains.Application.Dtos;
using ScotlandsMountains.Application.Ports;

namespace ScotlandsMountains.Application.UseCases.Mountains;

public record UploadDobihFileCommand(Stream Content) : IRequest<Result>;

internal class UploadDobihFileCommandHandler : IRequestHandler<UploadDobihFileCommand, Result>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IFileUploadNotificationService _fileUploadNotificationService;

    public UploadDobihFileCommandHandler(
        IFileStorageService fileStorageService,
        IFileUploadNotificationService fileUploadNotificationService)
    {
        _fileStorageService = fileStorageService;
        this._fileUploadNotificationService = fileUploadNotificationService;
    }

    public async Task<Result> HandleAsync(UploadDobihFileCommand request, CancellationToken cancellationToken = default)
    {
        const string containerName = "dobih-files";
        var fileName = Guid.NewGuid().ToString();

        var uri = await _fileStorageService.UploadFileAsync(containerName, fileName, request.Content, cancellationToken);

        await _fileUploadNotificationService.PublishFileUploadedNotificationAsync("Dobih", containerName, fileName, cancellationToken);

        return Result.Success();
    }
}
