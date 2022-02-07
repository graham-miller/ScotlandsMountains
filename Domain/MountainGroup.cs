namespace ScotlandsMountains.Domain;

public class MountainGroup : Entity
{
    public List<MountainSummary> Mountains { get; set; } = new();

    public int MountainsCount { get; set; } = 0;
}