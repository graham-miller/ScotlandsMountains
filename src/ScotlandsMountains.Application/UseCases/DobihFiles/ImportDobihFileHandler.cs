using ScotlandsMountains.Application.Adapters;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.UseCases.DobihFiles.Factories;
using ScotlandsMountains.Application.UseCases.DobihFiles.Models;

namespace ScotlandsMountains.Application.UseCases.DobihFiles;

public record ImportDobihFileCommand(string ContainerName, string FileName) : IRequest<Result>;

internal class ImportDobihFileCommandHandler : IRequestHandler<ImportDobihFileCommand, Result>
{
    private readonly IDobihImportService _dobihImportService;
    private readonly IFileStorageService _fileStorageService;

    public ImportDobihFileCommandHandler(
        IDobihImportService dobihImportService,
        IFileStorageService fileStorageService)
    {
        _dobihImportService = dobihImportService;
        _fileStorageService = fileStorageService;
    }

    public async Task<Result> HandleAsync(ImportDobihFileCommand request, CancellationToken cancellationToken = default)
    {
        await _dobihImportService.StartProcessingAsync(request.ContainerName, request.FileName, cancellationToken);

        var stream = await _fileStorageService.DownloadFileAsync(request.ContainerName, request.FileName, cancellationToken);
        var records = new DobihRecordsByNumber(stream);
        var regions = RegionsFactory.BuildFrom(records);
        var maps = MapsFactory.Build();
        var classifications = ClassificationsFactory.Build();
        var counties = CountiesFactory.BuildFrom(records);
        var countries = CountriesFactory.Build();
        var mountains = new MountainsFactory(regions, maps, classifications, counties, countries).BuildFrom(records);

        await _dobihImportService.CompleteProcessingAsync(
            request.ContainerName, request.FileName, records.FileName,
            regions, maps, classifications, counties, countries, mountains,
            cancellationToken);

        return Result.Success();
    }
}
