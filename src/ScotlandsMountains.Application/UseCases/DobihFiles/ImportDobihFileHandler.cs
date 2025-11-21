using Microsoft.EntityFrameworkCore;
using ScotlandsMountains.Application.Adapters;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.UseCases.DobihFiles.Factories;
using ScotlandsMountains.Application.UseCases.DobihFiles.Models;

namespace ScotlandsMountains.Application.UseCases.DobihFiles;

public record ImportDobihFileCommand(string ContainerName, string FileName) : IRequest<Result>;

internal class ImportDobihFileCommandHandler : IRequestHandler<ImportDobihFileCommand, Result>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IScotlandsMountainsDbContext _context;

    public ImportDobihFileCommandHandler(
        IFileStorageService fileStorageService,
        IScotlandsMountainsDbContext context)
    {
        _fileStorageService = fileStorageService;
        _context = context;
    }

    public async Task<Result> HandleAsync(ImportDobihFileCommand request, CancellationToken cancellationToken = default)
    {
        var file = await _context.DobihFiles.SingleAsync(f => f.ContainerName == request.ContainerName && f.FileName == request.FileName);
        file.StartProcessing();
        await _context.SaveChangesAsync(cancellationToken);

        var stream = await _fileStorageService.DownloadFileAsync(request.ContainerName, request.FileName, cancellationToken);
        var records = new DobihRecordsByNumber(stream);
        var regions = RegionsFactory.BuildFrom(records);
        var maps = MapsFactory.Build();
        var classifications = ClassificationsFactory.Build();
        var counties = CountiesFactory.BuildFrom(records);
        var countries = CountriesFactory.Build();
        var mountains = new MountainsFactory(regions, maps, classifications, counties, countries).BuildFrom(records);

        _context.Regions.AddRange(regions);
        _context.Maps.AddRange(maps);
        _context.Classifications.AddRange(classifications);
        _context.Counties.AddRange(counties);
        _context.Countries.AddRange(countries);
        _context.Mountains.AddRange(mountains);

        file.CompleteProcessing(records.FileName);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
