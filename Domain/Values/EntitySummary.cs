namespace ScotlandsMountains.Domain.Values;

public class EntitySummary
{
    public EntitySummary(Entity entity)
    {
        Id = entity.Id;
        Name = entity.Name;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }
}