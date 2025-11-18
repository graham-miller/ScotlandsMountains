namespace ScotlandsMountains.Application.Ports;

public interface IFileUploadNotificationService
{
    Task PublishFileUploadedNotificationAsync(string fileType, Uri fileUri, CancellationToken cancellationToken);
}
