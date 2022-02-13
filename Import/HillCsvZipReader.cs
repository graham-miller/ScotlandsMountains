namespace ScotlandsMountains.Import;

public class HillCsvZipReader : IMountainData
{
    private readonly ILogger _logger;
    private Dictionary<int, DobihRecord>? _records;
    private Dictionary<string, Section>? _sections;
    private List<County> _counties = new();
    private readonly ClassificationsProvider _classificationsProvider = new ();
    private CountiesProvider? _countiesProvider;
    private Dictionary<string, Map>? _maps1To50K;
    private Dictionary<string, Map>? _maps1To25K;

    public HillCsvZipReader(ILogger logger)
    {
        _logger = logger;
    }

    public List<Mountain> Mountains { get; private set; } = new();
    public List<Classification> Classifications { get; private set; } = new();
    public List<Section> Sections { get; private set; } = new();
    public List<County> Counties { get; private set; } = new();
    public List<Map> Maps { get; private set; } = new();

    public void Read()
    {
        _logger.LogInformation("Started reading hillcsv.zip");

        ReadRecords();
        GetSections();
        GetCounties();
        _countiesProvider = new CountiesProvider(_counties);
        GetMaps();
        CreateEntityLinks();
        UpdateMountainGroups();

        // WriteDobihRecordsToFile();
        _logger.LogInformation("Completed reading hillcsv.zip");
    }

    private void ReadRecords()
    {
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);

        using var zipArchive = new ZipArchive(FileStreams.HillCsvZip);
        using var csvStream = zipArchive.Entries[0].Open();
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, csvConfiguration);
        
        csv.Context.RegisterClassMap<DobihRecordMap>();

        _records = csv.GetRecords<DobihRecord>()
            .Where(r => r.Countries.Contains("Scotland"))
            .OrderByDescending(r => r.Metres)
            .ToDictionary(x => x.Number);
    }

    private void GetSections()
    {
        _sections = _records!.Values
            .Select(x => x.Region)
            .Distinct()
            .OrderBy(x => x)
            .ToDictionary(x => x, x => x.ToSection());
    }

    private void GetCounties()
    {
        _counties = _records!.Values
            .SelectMany(x => x.County?.SplitCounties() ?? new List<string>())
            .Distinct()
            .Select(x => x.ToCounty())
            .OrderBy(x => x.Name)
            .ToList();
    }

    private void GetMaps()
    {
        _maps1To50K = _records!.Values
            .SelectMany(x => x.Maps1To50K)
            .Distinct()
            .OrderBy(x => x)
            .ToDictionary(x => x, x => x.ToLandrangerMap());

        _maps1To25K = _records.Values
            .SelectMany(x => x.Maps1To25K)
            .Distinct()
            .OrderBy(x => x)
            .ToDictionary(x => x, x => x.ToExplorerMap());
    }

    private void CreateEntityLinks()
    {
        var mountainDobihLinks = new Dictionary<int, MountainDobihLink>();

        foreach (var @record in _records!.Values)
        {
            var mountain = @record.ToMountain();
            var link = new MountainDobihLink(mountain, @record);

            LinkClassifications(link);
            LinkSection(link);
            LinkCounties(link);
            LinkMaps(link);

            mountainDobihLinks.Add(link.Record.Number, link);
        }

        LinkParentMountains(mountainDobihLinks);

        Mountains = mountainDobihLinks.Values
            .Select(x => x.Mountain)
            .OrderByDescending(x => x.Height.Metres)
            .ToList();
        Classifications = _classificationsProvider.GetClassifications();
        Sections = _sections!.Values.OrderBy(x => x.Code).ToList();
        Counties = _counties;
        Maps = _maps1To50K!.Values.Concat(_maps1To25K!.Values).ToList();
    }

    private void LinkClassifications(MountainDobihLink link)
    {
        var classifications = _classificationsProvider.GetClassifications(link.Record);

        link.Mountain.Classifications = classifications
            .OrderBy(x => x.DisplayOrder)
            .Select(x => new ClassificationSummary(x))
            .ToList();

        foreach (var classification in classifications)
        {
            classification.Mountains.Add(new MountainSummary(link.Mountain));
        }
    }

    private void LinkSection(MountainDobihLink link)
    {
        var section = _sections![link.Record.Region];
        
        link.Mountain.Section = new EntitySummary(section);
        
        section.Mountains.Add(new MountainSummary(link.Mountain));
    }

    private void LinkCounties(MountainDobihLink link)
    {
        var counties = _countiesProvider!.GetCounties(link.Record);

        link.Mountain.Counties = counties
            .OrderBy(x => x.Name)
            .Select(x => new EntitySummary(x))
            .ToList();

        foreach (var county in counties)
        {
            county.Mountains.Add(new MountainSummary(link.Mountain));
        }
    }

    private void LinkMaps(MountainDobihLink link)
    {
        var maps = link.Record.Maps1To50K
            .Select(x => _maps1To50K![x])
            .Concat(
                link.Record.Maps1To25K
                    .Select(x => _maps1To25K![x]))
            .ToList();

        link.Mountain.Maps = maps
            .Select(x => new EntitySummary(x))
            .ToList();

        foreach (var map in maps)
        {
            map.Mountains.Add(new MountainSummary(link.Mountain));
        }
    }

    private static void LinkParentMountains(Dictionary<int, MountainDobihLink> links)
    {
        foreach (var link in links.Values)
        {
            if (link.Record.ParentSmc.HasValue && links.ContainsKey(link.Record.ParentSmc.Value))
            {
                link.Mountain.Parent = new MountainSummary(links[link.Record.ParentSmc.Value].Mountain);
            }
            else if (link.Record.ParentMa.HasValue && link.Record.ParentMa.Value != link.Record.Number && links.ContainsKey(link.Record.ParentMa.Value))
            {
                link.Mountain.Parent = new MountainSummary(links[link.Record.ParentMa.Value].Mountain);
            }
            else
            {
                link.Mountain.Parent = null;
            }
        }
    }

    private void UpdateMountainGroups()
    {
        UpdateMountainGroups(Classifications);
        UpdateMountainGroups(Counties);
        UpdateMountainGroups(Sections);
        UpdateMountainGroups(Maps);

        Classifications.ForEach(x => x.Description = string.Format(x.Description, x.MountainsCount));
    }

    private void UpdateMountainGroups<T>(List<T> groups) where T : MountainGroup
    {
        foreach (var @group in groups)
            @group.MountainsCount = @group.Mountains.Count;
    }

    // ReSharper disable once UnusedMember.Local
    private void WriteDobihRecordsToFile()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "mountains.json");
        var json = JsonSerializer.Serialize(Mountains, jsonOptions);
        File.WriteAllText(path, json);
    }

    private class MountainDobihLink
    {
        public MountainDobihLink(Mountain mountain, DobihRecord @record)
        {
            Mountain = mountain;
            Record = @record;
        }

        public Mountain Mountain { get; }

        public DobihRecord Record { get; }
    }
}