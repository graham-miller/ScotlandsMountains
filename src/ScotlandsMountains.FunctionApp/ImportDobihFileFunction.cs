using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.UseCases.DobihFiles;
using ScotlandsMountains.ServiceDefaults;
using System.Text.Json;

namespace ScotlandsMountains.FunctionApp;

public class FileUploadNotificationMessage
{
    public required string ContainerName { get; set; }
    public required string FileName { get; set; }
}

public class ImportDobihFileFunction
{
    private readonly IMediator _mediator; 
    private readonly ILogger<ImportDobihFileFunction> _logger;

    public ImportDobihFileFunction(
        IMediator mediator,
        ILoggerFactory loggerFactory)
    {
        _mediator = mediator;
        _logger = loggerFactory.CreateLogger<ImportDobihFileFunction>();
    }

    [Function(nameof(ImportDobihFileFunction))]
    public async Task Run(
        [ServiceBusTrigger(AspireConstants.FileUploadTopic, AspireConstants.FileUploadSubscription, Connection = AspireConstants.ServiceBus)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        var payload = JsonSerializer.Deserialize<FileUploadNotificationMessage>(message.Body);

        var command = new ImportDobihFileCommand(payload!.ContainerName, payload.FileName);

        await _mediator.SendAsync(command);
        
        await messageActions.CompleteMessageAsync(message);
    }
}
