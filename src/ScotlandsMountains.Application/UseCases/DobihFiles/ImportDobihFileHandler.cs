using ScotlandsMountains.Application.Adapters;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.UseCases.DobihFiles.Models;

namespace ScotlandsMountains.Application.UseCases.DobihFiles;

public record ImportDobihFileCommand(string ContainerName, string FileName) : IRequest<Result>;

internal class ImportDobihFileCommandHandler : IRequestHandler<ImportDobihFileCommand, Result>
{
    private readonly IFileStorageService _fileStorageService;

    public ImportDobihFileCommandHandler(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public async Task<Result> HandleAsync(ImportDobihFileCommand request, CancellationToken cancellationToken = default)
    {
        var stream = await _fileStorageService.DownloadFileAsync(request.ContainerName, request.FileName, cancellationToken);
 
        var file = new DobihFile(stream);

        return Result.Success();
    }
}
