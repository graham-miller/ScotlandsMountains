namespace ScotlandsMountains.FunctionApp.ProcessDobihFile.Models;

public class FileUploadNotificationMessage
{
    public required string FileType { get; set; }
    public required string ContainerName { get; set; }
    public required string FileName { get; set; }
    public DateTime UploadedAt { get; set; }
}
