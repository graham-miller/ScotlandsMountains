using CsvHelper.Configuration.Attributes;
using System.Reflection;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Parsing;

[Delimiter(",")]
[CultureInfo("en-GB")]
internal class DobihRecord
{
    [Name("Number")] public int Number { get; set; }

    [Name("Name")] public string Name { get; set; } = null!;

    [Name("Parent (SMC)")] public int? ParentSmc { get; set; }

    [Name("Parent name (SMC)")] public string? ParentNameSmc { get; set; }

    [Name("Section")] public string Section { get; set; } = null!;

    [Name("Region")] public string Region { get; set; } = null!;

    [Name("Area")] public string? Area { get; set; }

    [Name("Island")] public string? Island { get; set; }

    [Name("Topo Section")] public string TopoSection { get; set; } = null!;

    [Name("County")] public string? County { get; set; }

    [Name("Classification")] public string Classification { get; set; } = null!;

    [Name("Map 1:50k")] public string? Map1To50K { get; set; }

    [Name("Map 1:25k")] public string? Map1To25K { get; set; }

    [Name("Metres")] public decimal Metres { get; set; }

    [Name("Feet")] public decimal Feet { get; set; }

    [Name("Grid ref")] public string GridRef { get; set; } = null!;

    [Name("Grid ref 10")] public string? GridRef10 { get; set; }

    [Name("Drop")] public decimal Drop { get; set; }

    [Name("Col grid ref")] public string ColGridRef { get; set; } = null!;

    [Name("Col height")] public decimal ColHeight { get; set; }

    [Name("Feature")] public string? Feature { get; set; }

    [Name("Observations")] public string? Observations { get; set; }

    [Name("Survey")] public string? Survey { get; set; }

    [Name("Climbed")] public string? Climbed { get; set; }

    [Name("Country")] public string Country { get; set; } = null!;

    [Name("County Top")] public string? CountyTop { get; set; }

    [Name("Revision")] public DateTime Revision { get; set; }

    [Name("Comments")] public string? Comments { get; set; }

    [Name("Streetmap/MountainViews")] public string? StreetMapMountainViews { get; set; }

    [Name("Google Maps")] public string GoogleMaps { get; set; } = null!;

    [Name("Hill-bagging")] public string HillBagging { get; set; } = null!;

    [Name("Xcoord")] public int XCoord { get; set; }

    [Name("Ycoord")] public int YCoord { get; set; }

    [Name("Latitude")] public decimal Latitude { get; set; }

    [Name("Longitude")] public decimal Longitude { get; set; }

    [Name("GridrefXY")] public string GridRefXy { get; set; } = null!;

    [Name("_Section")] public string SectionNumber { get; set; } = null!;

    [Name("Parent (Ma)")] public int? ParentMa { get; set; }

    [Name("Parent name (Ma)")] public string? ParentNameMa { get; set; }

    [Name("MVNumber")] public int? MVNumber { get; set; }

    [Name("Ma")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Ma { get; set; }

    [Name("Ma=")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool MaTwin { get; set; }

    [Name("Hu")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Hu { get; set; }

    [Name("Hu=")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool HuTwin { get; set; }

    [Name("Tu")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Tu { get; set; }

    [Name("Sim")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Sim { get; set; }

    [Name("5")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Dodd { get; set; }

    [Name("M")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool M { get; set; }

    [Name("MT")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool MT { get; set; }

    [Name("F")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool F { get; set; }

    [Name("C")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool C { get; set; }

    [Name("G")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool G { get; set; }

    [Name("D")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool D { get; set; }

    [Name("DT")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool DT { get; set; }

    [Name("Hew")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Hew { get; set; }

    [Name("N")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool N { get; set; }

    [Name("Dew")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Dew { get; set; }

    [Name("DDew")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool DDew { get; set; }

    [Name("HF")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool HF { get; set; }

    [Name("4")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Tump4 { get; set; }

    [Name("3")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Tump3 { get; set; }

    [Name("2")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Tump2 { get; set; }

    [Name("1")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Tump1 { get; set; }

    [Name("0")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Tump0 { get; set; }

    [Name("W")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool W { get; set; }

    [Name("WO")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool WO { get; set; }

    [Name("B")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool B { get; set; }

    [Name("E")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool E { get; set; }

    [Name("HHB")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool HHB { get; set; }

    [Name("Sy")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Sy { get; set; }

    [Name("Fel")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Fel { get; set; }

    [Name("CoH")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool CoH { get; set; }

    [Name("CoH=")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool CoHTwin { get; set; }

    [Name("CoU")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool CoU { get; set; }

    [Name("CoU=")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool CoUTwin { get; set; }

    [Name("CoA")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool CoA { get; set; }

    [Name("CoA=")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool CoATwin { get; set; }

    [Name("CoL")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool CoL { get; set; }

    [Name("CoL=")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool CoLTwin { get; set; }

    [Name("SIB")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool SIB { get; set; }

    [Name("sMa")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool SMa { get; set; }

    [Name("sHu")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool SHu { get; set; }

    [Name("sSim")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool SSim { get; set; }

    [Name("s5")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool STump5 { get; set; }

    [Name("s4")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool STump4 { get; set; }

    [Name("Mur")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Mur { get; set; }

    [Name("CT")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool CT { get; set; }

    [Name("GT")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool GT { get; set; }

    [Name("BL")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool BL { get; set; }

    [Name("Bg")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Bg { get; set; }

    [Name("Y")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Y { get; set; }

    [Name("Cm")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Cm { get; set; }

    [Name("T100")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool T100 { get; set; }

    [Name("xMT")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool DeletedMT { get; set; }

    [Name("xC")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool DeletedC { get; set; }

    [Name("xG")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool DeletedG { get; set; }

    [Name("xN")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool DeletedN { get; set; }

    [Name("xDT")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool DeletedDT { get; set; }

    [Name("Dil")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Dil { get; set; }

    [Name("VL")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool VL { get; set; }

    [Name("A")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool A { get; set; }

    [Name("Ca")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Ca { get; set; }

    [Name("Bin")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Bin { get; set; }

    [Name("O")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool O { get; set; }

    [Name("Un")]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Un { get; set; }

    public (string name, List<string> aliases) GetNameAndAliases()
    {
        var name = "";
        var aliases = new List<string>();
        var alias = "";
        var inAlias = false;

        foreach (var letter in Name)
        {
            if (letter == '[')
            {
                alias = "";
                inAlias = true;
            }
            else if (letter == ']')
            {
                inAlias = false;
                aliases.Add(alias.Trim());
            }
            else if (inAlias)
            {
                alias += letter;
            }
            else
            {
                name += letter;
            }
        }

        return (name.Trim(), aliases);
    }

    public string? GetCleanFeature() => CleanSentence(Feature);

    public string? GetCleanObservations() => CleanSentence(Observations);

    private static string? CleanSentence(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;

        if (!s.EndsWith('.')) s += '.';

        return string.Concat(s[0].ToString().ToUpper(), s.AsSpan(1));
    }

    public string GetCleanGridRef() => CleanGridRef(GridRef);

    public string GetCleanColGridRef()
    {
        if (ColGridRef.Any(char.IsDigit))
            return CleanGridRef(ColGridRef);

        return ColGridRef.Trim().ToLower();
    }

    private static string CleanGridRef(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return string.Empty;

        var spacesRemoved = s.Replace(" ", "");

        var letters = spacesRemoved.Where(char.IsLetter).ToList();
        var numbers = spacesRemoved.Where(char.IsNumber).ToList();

        if (letters.Count > 2 || numbers.Count < 6 || !spacesRemoved.Take(letters.Count).SequenceEqual(letters))
            return s;

        return new string(letters.Concat(numbers.Take(3)).Concat(numbers.Skip(numbers.Count / 2).Take(3)).ToArray());
    }

    public List<string> Filter(List<string> classificationsCodes)
    {
        return GetType()
            .GetProperties()
            .Select(property => Filter(property, classificationsCodes))
            .Where(classificationCode => classificationCode != null)
            .ToList()!;
    }

    private string? Filter(PropertyInfo property, List<string> classificationsCodes)
    {
        var attribute = property.GetCustomAttribute(typeof(NameAttribute)) as NameAttribute;
        if (attribute == null || attribute.Names.Length != 1) return null;

        var classificationCode = attribute.Names[0];

        return classificationsCodes.Contains(classificationCode) && (bool)property.GetValue(this)!
            ? classificationCode
            : null;
    }
}