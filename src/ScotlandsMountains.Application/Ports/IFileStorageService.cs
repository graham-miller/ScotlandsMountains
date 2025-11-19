namespace ScotlandsMountains.Application.Ports;

public interface IFileStorageService
{
    Task<Uri> UploadFileAsync(string containerName, string fileName, Stream content, CancellationToken cancellationToken);

    Task<Stream> DownloadFileAsync(string containerName, string fileName, CancellationToken cancellationToken);
}
