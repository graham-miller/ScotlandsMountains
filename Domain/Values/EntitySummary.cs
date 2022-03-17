namespace ScotlandsMountains.Domain.Values;

public class EntitySummary
{
    public EntitySummary() { }

    public EntitySummary(Entity classification)
    {
        Id = classification.Id;
        Name = classification.Name;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }
}