using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ScotlandsMountains.FunctionApp.ProcessDobihFile.Models;
using System.Text.Json;

namespace ScotlandsMountains.FunctionApp.ProcessDobihFile;

public class ProcessDobihFileFunction
{
    private readonly ILogger<ProcessDobihFileFunction> _logger;
    private readonly BlobServiceClient _blobServiceClient;

    public ProcessDobihFileFunction(
        BlobServiceClient blobServiceClient,
        ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ProcessDobihFileFunction>();
        _blobServiceClient = blobServiceClient;
    }

    [Function(nameof(ProcessDobihFileFunction))]
    public async Task Run(
        [ServiceBusTrigger("file-upload-topic", "file-upload-sub", Connection = "messaging")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        var payload = JsonSerializer.Deserialize<FileUploadNotificationMessage>(message.Body);

        var container = _blobServiceClient.GetBlobContainerClient(payload!.ContainerName);
        var file = container.GetBlobClient(payload.FileName);

        using var stream = new MemoryStream();
        file.DownloadTo(stream);
        stream.Position = 0;

        var records = new DobihFile(stream);

        await messageActions.CompleteMessageAsync(message);
    }
}
