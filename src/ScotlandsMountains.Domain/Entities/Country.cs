namespace ScotlandsMountains.Domain.Entities;

public class Country : Entity
{
    private Country() { }

    public Country(string name, int displayOrder, char dobihCode)
    {
        Name = name;
        DisplayOrder = displayOrder;
        DobihCode = dobihCode;
    }

    public string Name { get; private set; } = null!;

    public int DisplayOrder { get; private set; }

    public char DobihCode { get; private set; }
}