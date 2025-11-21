using ScotlandsMountains.Application.Adapters;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.UseCases.DobihFiles;

public record UploadDobihFileCommand(Stream Content) : IRequest<Result>;

public class UploadDobihFileCommandHandler : IRequestHandler<UploadDobihFileCommand, Result>
{
    private readonly IScotlandsMountainsDbContext _context;
    private readonly IFileStorageService _fileStorageService;
    private readonly IFileUploadNotificationService _fileUploadNotificationService;

    public UploadDobihFileCommandHandler(
        IScotlandsMountainsDbContext context,
        IFileStorageService fileStorageService,
        IFileUploadNotificationService fileUploadNotificationService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
        _fileUploadNotificationService = fileUploadNotificationService;
    }

    public async Task<Result> HandleAsync(UploadDobihFileCommand request, CancellationToken cancellationToken = default)
    {
        const string containerName = "dobih-files";
        var fileName = Guid.NewGuid().ToString();
        var file = new DobihFile(containerName, fileName);

        _context.DobihFiles.Add(file);
        await _context.SaveChangesAsync(cancellationToken);

        var uri = await _fileStorageService.UploadFileAsync(containerName, fileName, request.Content, cancellationToken);

        await _fileUploadNotificationService.PublishFileUploadedNotificationAsync(containerName, fileName, cancellationToken);

        return Result.Success();
    }
}
