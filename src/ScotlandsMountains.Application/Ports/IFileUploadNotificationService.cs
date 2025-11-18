namespace ScotlandsMountains.Application.Ports;

public interface IFileUploadNotificationService
{
    Task PublishFileUploadedNotificationAsync(string fileType, string containerName, string fileName, CancellationToken cancellationToken);
}
