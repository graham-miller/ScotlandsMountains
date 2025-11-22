namespace ScotlandsMountains.Application.Adapters;

public class DobihFileModel
{
    public DobihFileModel(DobihFileDto file)
    {
        Name = file.Name;
        Status = file.Status;
        UploadedAt = file.UploadedAt;
        StartedProcessingAt = file.StartedProcessingAt;
        CompletedProcessingAt = file.CompletedProcessingAt;
    }
    
    public string? Name { get; private set; }

    public string Status { get; private set; }

    public DateTime UploadedAt { get; private set; }

    public DateTime? StartedProcessingAt { get; private set; }

    public DateTime? CompletedProcessingAt { get; private set; }
}
