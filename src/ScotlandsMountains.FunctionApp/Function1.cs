using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ScotlandsMountains.FunctionApp;

public class BlobMessage
{
    public required string FileType { get; set; }
    public required string ContainerName { get; set; }
    public required string FileName { get; set; }
    public DateTime UploadedAt { get; set; }
}

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;
    }

    [Function(nameof(Function1))]
    public async Task Run(
        [ServiceBusTrigger("file-upload-topic", "file-upload-sub", Connection = "messaging")]
        BlobMessage payload,
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        [BlobInput("{ContainerName}/{FileName}", Connection = "blobs")]
        Stream stream)
    {
        await messageActions.CompleteMessageAsync(message);
    }
}