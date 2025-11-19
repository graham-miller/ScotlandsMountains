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
        var stream = await _fileStorageService.DownloadFileAsync(request.ContainerName, request.FileName, cancellationToken);
        var file = new DobihRecordsByNumber(stream);

        var regions = RegionsFactory.BuildFrom(file);
        _context.Regions.AddRange(regions);

        var maps = MapsFactory.BuildFrom(file);
        _context.Maps.AddRange(maps);

        var classifications = ClassificationsFactory.Build();
        _context.Classifications.AddRange(classifications);

        var counties = CountiesFactory.BuildFrom(file);
        _context.Counties.AddRange(counties);

        var countries = CountriesFactory.Build();
        _context.Countries.AddRange(countries);

        var mountains = new MountainsFactory(_context).BuildFrom(file);
        _context.Mountains.AddRange(mountains);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
