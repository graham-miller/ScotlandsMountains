namespace ScotlandsMountains.Domain.Entities;

public class DobihFile : Entity
{
    private DobihFile() { }

    public DobihFile(string dobihName)
    {
        DobihName = dobihName;
        Status = DobihFileStatus.Pending;
    }

    public string DobihName { get; private set; } = null!;

    public DobihFileStatus Status { get; private set; }

    public void StartProcessing()
    {
        Status = DobihFileStatus.Processing;
    }

    public void CompleteProcessing()
    {
        Status = DobihFileStatus.Completed;
    }
}

public enum DobihFileStatus
{
    Pending,
    Processing,
    Completed
}