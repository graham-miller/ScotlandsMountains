namespace ScotlandsMountains.Domain;

public abstract class Entity
{
    protected Entity()
    {
        Id = Guid.NewGuid();
        PartitionKey = GetType().Name.Camelize();
    }

    public Guid Id { get; set; }

    public string PartitionKey { get; set; }

    public string Name { get; set; }
}