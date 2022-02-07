namespace ScotlandsMountains.Domain;

public class Map : MountainGroup
{
    public string Code { get; set; }

    public string Publisher { get; set; }

    public string Series { get; set; }

    public decimal Scale { get; set; }
}
