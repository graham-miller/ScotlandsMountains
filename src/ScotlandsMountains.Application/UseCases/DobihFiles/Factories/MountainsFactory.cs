using ScotlandsMountains.Application.UseCases.DobihFiles.Models;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Factories;

internal class MountainsFactory
{
    private readonly Dictionary<char, Country> _countryLookUp = [];
    private readonly Dictionary<string, County> _countyLookUp = [];
    private readonly Dictionary<string, Region> _regionLookup = [];
    private readonly Dictionary<string, Map> _explorerMapLookup = [];
    private readonly Dictionary<string, Map> _landrangerMapLookup = [];
    private readonly Dictionary<string, Classification> _classificationLookup = [];
    private readonly List<string> _classificationsCodes = [];
    private readonly Dictionary<int, Mountain> _parents = [];

    public MountainsFactory(List<Region> regions, List<Map> maps, List<Classification> classifications, List<County> counties, List<Country> countries)
    {
        _countryLookUp = countries.ToDictionary(x => x.DobihCode);
        _countyLookUp = counties.ToDictionary(x => x.Name);
        _regionLookup = regions.ToDictionary(x => x.DobihCode);
        _explorerMapLookup = maps.Where(x => x.Series.Name == MapSeries.Explorer).ToDictionary(x => x.Code);
        _landrangerMapLookup = maps.Where(x => x.Series.Name == MapSeries.Landranger).ToDictionary(x => x.Code);
        _classificationLookup = classifications.ToDictionary(x => x.DobihCode);
        _classificationsCodes = _classificationLookup.Keys.ToList();
    }

    internal List<Mountain> BuildFrom(DobihRecordsByNumber file)
    {
        return file.All
            .Select(CreateFrom)
            .ToList();
    }

    public Mountain CreateFrom(DobihRecord record)
    {
        var (name, aliases) = record.GetNameAndAliases();
        var region = _regionLookup[record.Region];

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
            .Select(c => _countryLookUp[c])
            .ForEach(mountain.AddCountry);

        record.County?.Trim()
            .Split('/')
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(c => _countyLookUp[c])
            .ForEach(mountain.AddCounty);

        (record.Map1To25K?
            .Split(' ')
            .Where(_explorerMapLookup.ContainsKey)
            .Select(c => _explorerMapLookup[c]) ?? [])
            .ForEach(mountain.AddMap);

        (record.Map1To50K?
            .Split(' ')
            .Where(_landrangerMapLookup.ContainsKey)
            .Select(c => _landrangerMapLookup[c]) ?? [])
            .ForEach(mountain.AddMap);

        record.Filter(_classificationsCodes)
            .Select(code => _classificationLookup[code])
            .ForEach(mountain.AddClassification);

        record.Classification
            .Split(',')
            .Where(_classificationLookup.ContainsKey)
            .Select(c => _classificationLookup[c])
            .ForEach(mountain.AddClassification);

        if (record.ParentSmc.HasValue) mountain.SetParent(_parents[record.ParentSmc.Value]);

        _parents.Add(mountain.DobihNumber, mountain);

        return mountain;
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