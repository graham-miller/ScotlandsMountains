using Azure.Messaging.ServiceBus;
using ScotlandsMountains.Application.Ports;

public class AzureServiceBusNotificationService : IFileUploadNotificationService
{
    private readonly ServiceBusClient _client;
    
    private const string TopicName = "file-upload-topic";

    public AzureServiceBusNotificationService(ServiceBusClient client)
    {
        _client = client;
    }

    public async Task PublishFileUploadedNotificationAsync(string fileType, Uri fileUri, CancellationToken cancellationToken = default)
    {
        ServiceBusSender sender = _client.CreateSender(TopicName);

        var notification = new { FileType = fileType, Url = fileUri.ToString(), UploadedAt = DateTimeOffset.UtcNow };
        var jsonPayload = System.Text.Json.JsonSerializer.Serialize(notification);

        var message = new ServiceBusMessage(jsonPayload);

        await sender.SendMessageAsync(message, cancellationToken);
    }
}