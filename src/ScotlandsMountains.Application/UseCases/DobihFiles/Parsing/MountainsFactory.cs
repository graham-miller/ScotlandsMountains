using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Parsing;

internal interface IMountainsFactory
{
    List<Mountain> BuildFrom(
        DobihRecords records,
        List<Region> regions,
        List<Map> maps,
        List<Classification> classifications,
        List<County> counties,
        List<Country> countries);
}

internal class MountainsFactory : IMountainsFactory
{
    public List<Mountain> BuildFrom(
        DobihRecords records,
        List<Region> regions,
        List<Map> maps,
        List<Classification> classifications,
        List<County> counties,
        List<Country> countries)
    {
        var lookups = new Lookups(regions, maps, classifications, counties, countries);
        var parents = new Dictionary<int, Mountain>();

        return records
            .Select(r => BuildFrom(r, lookups, parents))
            .ToList();
    }

    private Mountain BuildFrom(DobihRecord record, Lookups lookups, Dictionary<int, Mountain> parents)
    {
        var (name, aliases) = record.GetNameAndAliases();
        var region = lookups.RegionLookup[record.Region];

        var mountain = new Mountain(
            name,
            aliases,
            record.Metres,
            record.GetCleanGridRef(),
            record.GetCleanFeature(),
            record.GetCleanObservations(),
            record.Latitude,
            record.Longitude,
            record.Drop,
            record.GetCleanColGridRef(),
            record.ColHeight,
            record.Number,
            region);

        record.Country
            .ToCharArray()
            .Select(c => lookups.CountryLookUp[c])
            .ForEach(mountain.AddCountry);

        record.County?.Trim()
            .Split('/')
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(c => lookups.CountyLookUp[c])
            .ForEach(mountain.AddCounty);

        (record.Map1To25K?
            .Split(' ')
            .Where(lookups.ExplorerMapLookup.ContainsKey)
            .Select(c => lookups.ExplorerMapLookup[c]) ?? [])
            .ForEach(mountain.AddMap);

        (record.Map1To50K?
            .Split(' ')
            .Where(lookups.LandrangerMapLookup.ContainsKey)
            .Select(c => lookups.LandrangerMapLookup[c]) ?? [])
            .ForEach(mountain.AddMap);

        record.Filter(lookups.ClassificationsCodes)
            .Select(code => lookups.ClassificationLookup[code])
            .ForEach(mountain.AddClassification);

        record.Classification
            .Split(',')
            .Where(lookups.ClassificationLookup.ContainsKey)
            .Select(c => lookups.ClassificationLookup[c])
            .ForEach(mountain.AddClassification);

        if (record.ParentSmc.HasValue) mountain.SetParent(parents[record.ParentSmc.Value]);

        parents.Add(mountain.DobihNumber, mountain);

        return mountain;
    }

    private class Lookups
    {
        public Lookups(
            List<Region> regions,
            List<Map> maps,
            List<Classification> classifications,
            List<County> counties,
            List<Country> countries)
        {
            CountryLookUp = countries.ToDictionary(x => x.DobihCode);
            CountyLookUp = counties.ToDictionary(x => x.Name);
            RegionLookup = regions.ToDictionary(x => x.DobihCode);
            ExplorerMapLookup = maps.Where(x => x.Series.Name == MapSeries.Explorer).ToDictionary(x => x.Code);
            LandrangerMapLookup = maps.Where(x => x.Series.Name == MapSeries.Landranger).ToDictionary(x => x.Code);
            ClassificationLookup = classifications.ToDictionary(x => x.DobihCode);
            ClassificationsCodes = [.. ClassificationLookup.Keys];
        }

        public Dictionary<char, Country> CountryLookUp { get; private set; }
        public Dictionary<string, County> CountyLookUp { get; private set; }
        public Dictionary<string, Region> RegionLookup { get; private set; }
        public Dictionary<string, Map> ExplorerMapLookup { get; private set; }
        public Dictionary<string, Map> LandrangerMapLookup { get; private set; }
        public Dictionary<string, Classification> ClassificationLookup { get; private set; }
        public List<string> ClassificationsCodes { get; private set; }
    }

}

internal static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }
}