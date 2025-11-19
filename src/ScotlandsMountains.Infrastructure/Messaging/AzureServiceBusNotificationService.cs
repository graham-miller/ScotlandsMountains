using Azure.Messaging.ServiceBus;
using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.ServiceDefaults;

public class AzureServiceBusNotificationService : IFileUploadNotificationService
{
    private readonly ServiceBusClient _client;
    
    public AzureServiceBusNotificationService(ServiceBusClient client)
    {
        _client = client;
    }

    public async Task PublishFileUploadedNotificationAsync(string containerName, string fileName, CancellationToken cancellationToken)
    {
        ServiceBusSender sender = _client.CreateSender(AspireConstants.FileUploadTopic);

        var notification = new {
            ContainerName = containerName, 
            FileName = fileName, 
        };

        var jsonPayload = System.Text.Json.JsonSerializer.Serialize(notification);

        var message = new ServiceBusMessage(jsonPayload);

        await sender.SendMessageAsync(message, cancellationToken);
    }
}