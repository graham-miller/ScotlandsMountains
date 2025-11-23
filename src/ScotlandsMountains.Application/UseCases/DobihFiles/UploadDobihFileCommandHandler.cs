using ScotlandsMountains.Application.Adapters;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Shared;

namespace ScotlandsMountains.Application.UseCases.DobihFiles;

public record UploadDobihFileCommand(Stream Content) : IRequest<Result<DobihFileDto>>;

public class UploadDobihFileCommandHandler : IRequestHandler<UploadDobihFileCommand, Result<DobihFileDto>>
{
    private readonly IDobihImportService _dobihImportService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IFileUploadNotificationService _fileUploadNotificationService;

    public UploadDobihFileCommandHandler(
        IDobihImportService dobihImportService,
        IFileStorageService fileStorageService,
        IFileUploadNotificationService fileUploadNotificationService)
    {
        _dobihImportService = dobihImportService;
        _fileStorageService = fileStorageService;
        _fileUploadNotificationService = fileUploadNotificationService;
    }

    public async Task<Result<DobihFileDto>> HandleAsync(UploadDobihFileCommand request, CancellationToken cancellationToken = default)
    {
        const string containerName = "dobih-files";
        var fileName = Guid.NewGuid().ToString();

        try
        {

            var file = await _dobihImportService.AcceptUploadAsync(containerName, fileName, cancellationToken);
            var uri = await _fileStorageService.UploadFileAsync(containerName, fileName, request.Content, cancellationToken);

            await _fileUploadNotificationService.PublishFileUploadedNotificationAsync(containerName, fileName, cancellationToken);

            return Result.Success(new DobihFileDto(file));
        }
        catch (InvalidOperationException)
        {
            return Result.Failure<DobihFileDto>(Errors.BadRequest);
        }
        catch (Exception)
        {
            return Result.Failure<DobihFileDto>(Errors.Unknown);
        }
    }
}
