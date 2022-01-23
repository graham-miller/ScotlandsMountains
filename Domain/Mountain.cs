namespace ScotlandsMountains.Domain;

public class Mountain
{
    public string PartitionKey { get; set; }

    public DateTime IngestedAtUtc { get; set; }
}
