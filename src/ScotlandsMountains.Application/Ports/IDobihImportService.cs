using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.Ports;

public interface IDobihImportService
{
    Task<DobihFile?> GetDobihFileAsync(int id, CancellationToken cancellationToken);

    Task<DobihFile> AcceptUploadAsync(string containerName, string fileName, CancellationToken cancellationToken);
    
    Task<DobihFile> StartProcessingAsync(string containerName, string fileName, CancellationToken cancellationToken);
    
    Task<DobihFile> CompleteProcessingAsync(
        string containerName,
        string fileName,
        string dobihName,
        List<Region> regions,
        List<Map> maps,
        List<Classification> classifications,
        List<County> counties,
        List<Country> countries,
        List<Mountain> mountains,
        CancellationToken cancellationToken);
}
