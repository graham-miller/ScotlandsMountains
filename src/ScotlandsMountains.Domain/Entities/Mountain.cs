using ScotlandsMountains.Domain.Values;

namespace ScotlandsMountains.Domain.Entities;

public class Mountain : Entity
{
    private Mountain() { }

    public Mountain(
        string name, List<string> aliases, decimal height, string gridRef, string? feature, string? observations, decimal latitude, decimal longitude,
        decimal drop, string colGridRef, decimal colHeight, int dobihNumber, Region region)
    {
        Name = name;
        Aliases = aliases;
        Height = height;
        GridRef = gridRef;
        Feature = feature;
        Observations = observations;
        Coordinates = new Coordinates(latitude, longitude);
        Drop = drop;
        ColGridRef = colGridRef;
        ColHeight = colHeight;
        DobihNumber = dobihNumber;
        Region = region;
    }

    public string Name { get; set; } = null!;

    public List<string> Aliases { get; private set; } = [];

    public Height Height { get; private set; }

    public GridRef GridRef { get; private set; }

    public string? Feature { get; private set; }

    public string? Observations { get; private set; }

    public Coordinates Coordinates { get; private set; }

    public Height Drop { get; private set; }

    public GridRef ColGridRef { get; private set; }

    public Height ColHeight { get; private set; }

    public int DobihNumber { get; private set; }

    public List<Country> Countries { get; private set; } = [];

    public Region Region { get; private set; }

    public List<County> Counties { get; private set; } = [];

    public List<Map> Maps { get; private set; } = [];

    public List<Classification> Classifications { get; private set; } = [];

    public Mountain? Parent { get; private set; }

    public void AddCountry(Country country)
    {
        Countries.Add(country);
    }

    public void AddCounty(County county)
    {
        Counties.Add(county);
    }

    public void AddMap(Map map)
    {
        Maps.Add(map);
    }

    public void AddClassification(Classification classification)
    {
        Classifications.Add(classification);
    }

    public void SetParent(Mountain parent)
    {
        Parent = parent;
    }
}
