namespace ScotlandsMountains.Domain.Entities;

public class Region : Entity
{
    private Region() { }

    public Region(string code, string name, int displayOrder, string dobihCode)
    {
        Code = code;
        Name = name;
        DisplayOrder = displayOrder;
        DobihCode = dobihCode;
    }

    public string Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public int DisplayOrder { get; private set; }

    public string DobihCode { get; private set; } = null!;

    public List<Mountain> Mountains { get; private set; } = [];

    public override string ToString() => $"{Code}:{Name}";
}
