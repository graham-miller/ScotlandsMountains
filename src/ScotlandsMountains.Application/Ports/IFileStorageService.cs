namespace ScotlandsMountains.Application.Ports;

public interface IFileStorageService
{
    Task UploadFileAsync(string containerName, string fileName, Stream content, CancellationToken cancellationToken);
}
