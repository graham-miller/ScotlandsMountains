namespace ScotlandsMountains.Application.Ports;

public interface IFileUploadNotificationService
{
    Task PublishFileUploadedNotificationAsync(string containerName, string fileName, CancellationToken cancellationToken);
}
