namespace ScotlandsMountains.Domain.Values;

public class MountainSummary : EntitySummary
{
    public MountainSummary() { }

    public MountainSummary(Mountain mountain)
        : base(mountain)
    {
        Height = mountain.Height;
        Location = mountain.Location;
    }

    public Height Height { get; set; }

    public Location Location { get; set; }
}