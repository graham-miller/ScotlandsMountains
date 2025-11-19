namespace ScotlandsMountains.Domain.Entities;

public class Classification : Entity
{
    private Classification() { }

    public Classification(string name, string nameSingular, int displayOrder, string description, string dobihCode)
    {
        Name = name;
        NameSingular = nameSingular;
        DisplayOrder = displayOrder;
        Description = description;
        DobihCode = dobihCode;
    }

    public string Name { get; private set; } = null!;

    public string NameSingular { get; private set; } = null!;

    public int DisplayOrder { get; private set; }

    public string Description { get; private set; } = null!;

    public string DobihCode { get; private set; } = null!;

    public List<Mountain> Mountains { get; private set; } = [];

    public Classification AddMountain(Mountain mountain)
    {
        Mountains.Add(mountain);
        return this;
    }
}