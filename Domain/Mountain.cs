using Microsoft.Azure.Cosmos.Spatial;
using ScotlandsMountains.Domain.Values;

namespace ScotlandsMountains.Domain;

public class Mountain : Entity
{
    public List<string> Aliases { get; set; }

    public Point Location { get; set; }

    public int DobihId { get; set; }

    public string GridRef { get; set; }

    public Height Height { get; set; }

    public Prominence Prominence { get; set; }

    public string Features { get; set; }

    public string Observations { get; set; }

    public Mountain Parent { get; set; }

    public string Region { get; set; }

    public string County { get; set; }

    public List<string> Classifications { get; set; }

    public List<string> Maps { get; set; }
}