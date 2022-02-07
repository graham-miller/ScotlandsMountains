namespace ScotlandsMountains.Import.Dobih;

internal static class DobihRecordExtensions
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

    public static Mountain ToMountain(this DobihRecord @record)
    {
        return new Mountain
        {
            Name = @record.Name,
            Aliases = @record.Aliases,
            Location = new Location(@record.Longitude, @record.Latitude),
            DobihId = @record.Number,
            GridRef = @record.GridRef,
            Height = new Height{ Metres = @record.Metres },
            Prominence = new Prominence
            {
                MeasuredFrom = @record.ColGridRef,
                MeasuredFromHeight = new Height { Metres = @record.ColHeight },
                Metres = @record.Drop
            },
            Features = @record.Feature,
            Observations = @record.Observations
        };
    }

    private static Map ToMap(this string code, string series, string publisher, decimal scale)
    {
        return new Map
        {
            Name = $"OS {series} {code}",
            Code = code,
            Publisher = publisher,
            Series = series,
            Scale = scale
        };
    }
}