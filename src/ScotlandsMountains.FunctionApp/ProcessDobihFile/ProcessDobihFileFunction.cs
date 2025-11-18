using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ScotlandsMountains.FunctionApp.ProcessDobihFile.Models;

namespace ScotlandsMountains.FunctionApp.ProcessDobihFile;

public class ProcessDobihFileFunction
{
    private readonly ILogger<ProcessDobihFileFunction> _logger;

    public ProcessDobihFileFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ProcessDobihFileFunction>();
    }

    [Function(nameof(ProcessDobihFileFunction))]
    public async Task Run(
        [ServiceBusTrigger("file-upload-topic", "file-upload-sub", Connection = "messaging")]
        FileUploadNotificationMessage payload,
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        [BlobInput("{ContainerName}/{FileName}", Connection = "blobs")]
        Stream stream)
    {
        if (stream == null) return;

        var records = new DobihFile(stream);

        await messageActions.CompleteMessageAsync(message);
    }
}
