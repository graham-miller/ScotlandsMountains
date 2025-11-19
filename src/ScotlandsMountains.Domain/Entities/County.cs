namespace ScotlandsMountains.Domain.Entities;

public class County : Entity
{
    private County() { }

    public County(string name)
    {
        Name = name;
    }

    public string Name { get; private set; } = null!;
}