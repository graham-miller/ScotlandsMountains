namespace ScotlandsMountains.Domain.Values;

public class Location
{
    public Location(double longitude, double latitude)
    {
        Coordinates = new[] { longitude, latitude };
    }

    public string Type { get; set; } = "Point";
    
    public double[] Coordinates { get; set; }
}