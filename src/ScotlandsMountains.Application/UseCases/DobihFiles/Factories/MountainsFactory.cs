using ScotlandsMountains.Application.Ports;
using ScotlandsMountains.Application.UseCases.DobihFiles.Models;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Factories;

internal class MountainsFactory
{
    private readonly IScotlandsMountainsDbContext _context;

    private bool _isLookupsLoaded = false;
    private Dictionary<char, Country> _countryLookUp = [];
    private Dictionary<string, County> _countyLookUp = [];
    private Dictionary<string, Region> _regionLookup = [];
    private Dictionary<string, Map> _explorerMapLookup = [];
    private Dictionary<string, Map> _landrangerMapLookup = [];
    private Dictionary<string, Classification> _classificationLookup = [];
    private List<string> _classificationsCodes = [];
    private readonly Dictionary<int, Mountain> _parents = new();

    public MountainsFactory(IScotlandsMountainsDbContext context)
    {
        _context = context;
    }

    internal IEnumerable<Mountain> BuildFrom(DobihRecordsByNumber file)
    {
        foreach (var record in file.All)
        {
            yield return CreateFrom(record);
        }
    }

    public Mountain CreateFrom(DobihRecord record)
    {
        if (!_isLookupsLoaded) LoadLookups();

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

    private void LoadLookups()
    {
        _countryLookUp = _context.Countries.ToDictionary(x => x.DobihCode);
        _countyLookUp = _context.Counties.ToDictionary(x => x.Name);
        _regionLookup = _context.Regions.ToDictionary(x => x.DobihCode);
        _explorerMapLookup = _context.Maps.Where(x => x.Series.Name == MapSeries.Explorer).ToDictionary(x => x.Code);
        _landrangerMapLookup = _context.Maps.Where(x => x.Series.Name == MapSeries.Landranger).ToDictionary(x => x.Code);
        _classificationLookup = _context.Classifications.ToDictionary(x => x.DobihCode);
        _classificationsCodes = _classificationLookup.Keys.ToList();
        _isLookupsLoaded = true;
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