namespace ScotlandsMountains.Domain.Values;

public record struct Coordinates
{
    public Coordinates(string s)
    {
        var parts = s.Split(',');
        Latitude = decimal.Parse(parts[0]);
        Longitude = decimal.Parse(parts[1]);
    }

    public Coordinates(decimal latitude, decimal longitude)
    {
        if (latitude < -90m || latitude > 90m)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees.");
        }

        if (longitude < -180m || longitude > 180m)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees.");
        }
        
        Latitude = latitude;
        Longitude = longitude;
    }

    public decimal Latitude { get; }
    
    public decimal Longitude { get; }
    
    public override string ToString() => $"{Latitude:0.000000}, {Longitude:0.000000}";
}
