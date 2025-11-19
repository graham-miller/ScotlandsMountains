using ScotlandsMountains.Domain.Values;

namespace ScotlandsMountains.Domain.Entities;

public class Map : Entity
{
    private Map() { }

    public Map(string code, string name, string isbn, string isbnActive, MapSeries series)
    {
        Code = code;
        Name = name;
        Isbn = isbn;
        IsbnActive = isbnActive;
        Series = series;
    }

    public string Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string Isbn { get; private set; } = null!;

    public string IsbnActive { get; private set; } = null!;

    public MapSeries Series { get; private set; } = null!;

    public List<Mountain> Mountains { get; private set; } = [];
}

public class MapPublisher : Entity
{
    public const string OrdnanceSurvey = "Ordnance Survey";

    private MapPublisher() { }

    public MapPublisher(string name)
    {
        Name = name;
    }

    public string Name { get; private set; } = null!;
}

public class MapSeries : Entity
{
    public const string Explorer = "Explorer";
    public const string Landranger = "Landranger";

    private MapSeries() { }

    public MapSeries(string name, MapPublisher publisher, decimal scale)
    {
        Name = name;
        Publisher = publisher;
        Scale = scale;
    }

    public string Name { get; private set; } = null!;

    public MapPublisher Publisher { get; private set; } = null!;

    public MapScale Scale { get; private set; }
}