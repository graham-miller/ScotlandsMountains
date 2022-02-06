using ScotlandsMountains.Domain;
using ScotlandsMountains.Domain.Values;

namespace ScotlandsMountains.Import;

public class HillCsvZipImporter
{
    private Dictionary<int, DobihRecord>? _records;
    private Dictionary<string, Section>? _sections;
    private List<County> _counties = new();
    private readonly ClassificationsProvider _classificationsProvider = new ();
    private CountiesProvider? _countiesProvider;
    private Dictionary<string, Map>? _maps1To50K;
    private Dictionary<string, Map>? _maps1To25K;

    public List<Mountain> Mountains { get; set; } = new();
    public List<Classification> Classifications { get; set; } = new();
    public List<Section> Sections { get; set; } = new();
    public List<County> Counties { get; set; } = new();
    public List<Map> Maps { get; set; } = new();

    public void Import()
    {
        ReadRecords();
        GetSections();
        GetCounties();
        _countiesProvider = new CountiesProvider(_counties);
        GetMaps();
        CreateEntityLinks();

        // WriteDobihRecordsToFile(records);
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

        // TODO link mountains to Classifications
        // TODO link mountains to Sections
        // TODO link mountains to Counties
        // TODO link mountains to Maps

        Mountains = mountainDobihLinks.Values
            .Select(x => x.Mountain)
            .OrderByDescending(x => x.Height.Metres)
            .ToList();

        // TODO set Classifications
        // TODO set Sections
        // TODO set Counties
        // TODO set Maps
    }

    private void LinkClassifications(MountainDobihLink link)
    {
        link.Mountain.Classifications = _classificationsProvider.GetClassifications(link.Record)
            .OrderBy(x => x.DisplayOrder)
            .Select(x => new EntitySummary(x))
            .ToList();
    }

    private void LinkSection(MountainDobihLink link)
    {
        link.Mountain.Section = new EntitySummary(_sections[link.Record.Region]);
    }

    private void LinkCounties(MountainDobihLink link)
    {
        link.Mountain.Counties = _countiesProvider.GetCounties(link.Record)
            .OrderBy(x => x.Name)
            .Select(x => new EntitySummary(x))
            .ToList();
    }

    private void LinkMaps(MountainDobihLink link)
    {
        link.Mountain.Maps = link.Record.Maps1To50K
            .Select(x => new EntitySummary(_maps1To50K[x]))
            .OrderBy(x => x.Name)
            .Concat(
                link.Record.Maps1To25K
                    .Select(x => new EntitySummary(_maps1To25K[x]))
                    .OrderBy(x => x.Name)
                )
            .ToList();
    }

    private void LinkParentMountains(Dictionary<int, MountainDobihLink> links)
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

    // ReSharper disable once UnusedMember.Local
    private void WriteDobihRecordsToFile()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "mountain-records.json");
        var json = JsonSerializer.Serialize(_records, jsonOptions);
        File.WriteAllText(path, json);
    }

    private class MountainDobihLink
    {
        public MountainDobihLink(Mountain mountain, DobihRecord @record)
        {
            Mountain = mountain;
            Record = @record;
        }

        public Mountain Mountain { get; set; }

        public DobihRecord Record { get; set; }
    }
}