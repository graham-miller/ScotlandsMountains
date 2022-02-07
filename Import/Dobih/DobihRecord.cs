namespace ScotlandsMountains.Import.Dobih;

public class DobihRecord
{
    [Name("Number")]
    public int Number { get; set; }

    public string Name { get; set; }

    public List<string> Aliases { get; set; } = new();

    [Name("Parent (SMC)")]
    public int? ParentSmc { get; set; }

    [Name("Parent name (SMC)")]
    public string? ParentNameSmc { get; set; }

    [Name("Section")]
    public string Section { get; set; }

    [Name("Region")]
    public string Region { get; set; }

    [Name("Area")]
    public string? Area { get; set; }

    [Name("Island")]
    public string? Island { get; set; }

    [Name("Topo Section")]
    public string? TopoSection { get; set; }

    [Name("County")]
    public string? County { get; set; }

    [Name("Classification")]
    public List<string> Classifications { get; set; } = new();

    [Name("Map 1:50k")]
    public List<string> Maps1To50K { get; set; } = new();

    [Name("Map 1:25k")]
    public List<string> Maps1To25K { get; set; } = new();

    [Name("Metres")]
    public decimal Metres { get; set; }

    [Name("Feet")]
    public decimal Feet { get; set; }

    [Name("Grid ref")]
    public string GridRef { get; set; }

    [Name("Grid ref 10")]
    public string? GridRef10 { get; set; }

    [Name("Drop")]
    public decimal Drop { get; set; }

    [Name("Col grid ref")]
    public string ColGridRef { get; set; }

    [Name("Col height")]
    public decimal ColHeight { get; set; }

    [Name("Feature")]
    public string? Feature { get; set; }

    [Name("Observations")]
    public string? Observations { get; set; }

    [Name("Survey")]
    public string? Survey { get; set; }

    [Name("Climbed")]
    public string? Climbed { get; set; }

    [Name("Country")] public List<string> Countries { get; set; } = new();

    [Name("County Top")]
    public string? CountyTop { get; set; }

    [Name("Revision")]
    public DateTime Revision { get; set; }

    [Name("Comments")]
    public string? Comments { get; set; }

    [Name("Streetmap/MountainViews")]
    public string? StreetmapMountainViews { get; set; }

    [Name("Geograph")]
    public string? Geograph { get; set; }

    [Name("Hill-bagging")]
    public string? HillBagging { get; set; }

    [Name("Xcoord")]
    public int XCoord { get; set; }

    [Name("Ycoord")]
    public int YCoord { get; set; }

    [Name("Latitude")]
    public double Latitude { get; set; }

    [Name("Longitude")]
    public double Longitude { get; set; }

    [Name("GridrefXY")]
    public string GridRefXY { get; set; }

    [Name("_Section")]
    public string _Section { get; set; }

    [Name("Parent (Ma)")]
    public int? ParentMa { get; set; }

    [Name("Parent name (Ma)")]
    public string? ParentNameMa { get; set; }

    [Name("MVNumber")]
    public int? MvNumber { get; set; }

    public IsMemberOf IsMemberOf { get; set; } = new();
}