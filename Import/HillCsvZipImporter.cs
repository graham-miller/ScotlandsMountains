using ScotlandsMountains.Domain;

namespace ScotlandsMountains.Import;

public class HillCsvZipImporter
{
    private Dictionary<int, DobihRecord> _records = new();
    private Dictionary<string, Region> _regions = new();
    private Dictionary<string, County> _counties = new();
    private ClassificationsChecker _classificationsChecker = new ClassificationsChecker();
    private Dictionary<string, Map> _maps = new();

    public void Import()
    {
        ReadRecords();
        GetRegions();
        GetCounties();
        GetMaps();

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

    private void GetRegions()
    {
        _regions = _records.Values
            .Select(x => x.Region)
            .Distinct()
            .OrderBy(x => x)
            .ToDictionary(x => x, x => x.ToRegion());
    }

    private void GetCounties()
    {
        _counties = _records.Values
            .Select(x => x.County ?? Helper.MissingValue)
            .Distinct()
            .Where(x => !x.IsMissing())
            .OrderBy(x => x)
            .ToDictionary(x => x, x => x.ToCounty());
    }

    private void GetMaps()
    {
        var maps1To50K = _records.Values
            .SelectMany(x => x.Maps1To50K)
            .Distinct()
            .OrderBy(x => x)
            .ToDictionary(x => x, x => x.ToLandrangerMap());

        var maps1To25K = _records.Values
            .SelectMany(x => x.Maps1To25K)
            .Distinct()
            .OrderBy(x => x)
            .ToDictionary(x => x, x => x.ToExplorerMap());

        _maps = maps1To50K.Concat(maps1To25K).ToDictionary(x => x.Key, x => x.Value);
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
}