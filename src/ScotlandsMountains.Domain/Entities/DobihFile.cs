namespace ScotlandsMountains.Domain.Entities;

public class DobihFile : Entity
{
    private DobihFile() { }

    public DobihFile(string containerName, string fileName)
    {
        ContainerName = containerName;
        FileName = fileName;
        Status = DobihFileStatus.Pending;
        UploadedAt = DateTime.UtcNow;
    }

    public string ContainerName { get; private set; } = null!;

    public string FileName { get; private set; } = null!;

    public string? DobihName { get; private set; }

    public DobihFileStatus Status { get; private set; }

    public DateTime UploadedAt { get; private set; }

    public DateTime? StartedProcessingAt { get; private set; }

    public DateTime? CompletedProcessingAt { get; private set; }

    public void StartProcessing()
    {
        if (Status != DobihFileStatus.Pending)
            throw new InvalidOperationException("Cannot start processing a file that is not in Pending status.");

        Status = DobihFileStatus.Processing;
        StartedProcessingAt = DateTime.UtcNow;
    }

    public void CompleteProcessing(string dobihName)
    {
        if (Status != DobihFileStatus.Processing)
            throw new InvalidOperationException("Cannot complete processing a file that is not in Processing status.");

        DobihName = dobihName;
        Status = DobihFileStatus.Completed;
        CompletedProcessingAt = DateTime.UtcNow;
    }
}

public enum DobihFileStatus
{
    Pending,
    Processing,
    Completed,
}