using ScotlandsMountains.Domain;

namespace ScotlandsMountains.Import;

internal static class Helper
{
    private const decimal Scale1To50000 = (1m / 50_000m);
    private const decimal Scale1To25000 = (1m / 25_000m);

    public static Section ToSection(this string s)
    {
        return new Section
        {
            Code = s.Split(':')[0].Trim(),
            Name = s.Split(':')[1].Trim()
        };
    }

    public static List<string> SplitCounties(this string s)
    {
        return s.Split('/')
            .Select(x => x.Trim())
            .ToList();
    }

    public static County ToCounty(this string s)
    {
        return new County
        {
            Name = s
        };
    }

    public static Map ToLandrangerMap(this string s)
    {
        return s.ToMap("Landranger", "Ordnance Survey", Scale1To50000);
    }

    public static Map ToExplorerMap(this string s)
    {
        return s.ToMap("Explorer", "Ordnance Survey", Scale1To25000);
    }

    private static Map ToMap(this string code, string series, string publisher, decimal scale)
    {
        return new Map
        {
            Code = code,
            Publisher = publisher,
            Series = series,
            Scale = scale
        };
    }
}