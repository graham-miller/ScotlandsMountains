using Azure.Storage.Blobs;
using ScotlandsMountains.Application.Ports;

namespace ScotlandsMountains.Infrastructure.Storage;

internal class AzureBlobStorageService : IFileStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Uri> UploadFileAsync(string containerName, string fileName, Stream content, CancellationToken cancellationToken)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync();
        
        var blob = container.GetBlobClient(fileName);
        await blob.UploadAsync(content, cancellationToken);

        return blob.Uri;
    }
}
