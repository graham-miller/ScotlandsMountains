using Microsoft.EntityFrameworkCore;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Infrastructure.Database.Repositories;

internal class DobihImportService : IDobihImportService
{
    private readonly ScotlandsMountainsDbContext _context;

    public DobihImportService(ScotlandsMountainsDbContext context)
    {
        _context = context;
    }

    public async Task<DobihFile> AcceptUploadAsync(string containerName, string fileName, CancellationToken cancellationToken)
    {
        if (await _context.DobihFiles.AnyAsync())
            throw new InvalidOperationException("A DoBIH file has already been imported.");

        var file = new DobihFile(containerName, fileName);

        _context.DobihFiles.Add(file);

        await _context.SaveChangesAsync(cancellationToken);

        return file;
    }

    public async Task<DobihFile> StartProcessingAsync(string containerName, string fileName, CancellationToken cancellationToken)
    {
        var file = await GetDobihFileAsync(containerName, fileName, cancellationToken);
        file.StartProcessing();
        
        await _context.SaveChangesAsync(cancellationToken);

        return file;
    }

    public async Task<DobihFile> CompleteProcessingAsync(
        string containerName, string fileName,
        string dobihName,
        List<Region> regions,
        List<Map> maps,
        List<Classification> classifications,
        List<County> counties,
        List<Country> countries,
        List<Mountain> mountains,
        CancellationToken cancellationToken)
    {
        _context.Regions.AddRange(regions);
        _context.Maps.AddRange(maps);
        _context.Classifications.AddRange(classifications);
        _context.Counties.AddRange(counties);
        _context.Countries.AddRange(countries);
        _context.Mountains.AddRange(mountains);

        var file = await GetDobihFileAsync(containerName, fileName, cancellationToken);
        file.CompleteProcessing(dobihName);

        await _context.SaveChangesAsync(cancellationToken);

        return file;
    }

    private async Task<DobihFile> GetDobihFileAsync(string containerName, string fileName, CancellationToken cancellationToken)
    {
        return await _context.DobihFiles.SingleAsync(f => f.ContainerName == containerName && f.FileName == fileName, cancellationToken);
    }
}
