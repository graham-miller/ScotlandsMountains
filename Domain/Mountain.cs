namespace ScotlandsMountains.Domain;

public class Mountain : Entity
{
    public List<string> Aliases { get; set; }

    public Location Location { get; set; }

    public int DobihId { get; set; }

    public string GridRef { get; set; }

    public Height Height { get; set; }

    public Prominence Prominence { get; set; }

    public string Features { get; set; }

    public string Observations { get; set; }

    public MountainSummary? Parent { get; set; }

    public EntitySummary Section { get; set; }

    public List<EntitySummary> Counties { get; set; }

    public List<ClassificationSummary> Classifications { get; set; }

    public List<EntitySummary> Maps { get; set; }
}