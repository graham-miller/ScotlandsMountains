namespace ScotlandsMountains.Import;

public class HillCsvZipImporter
{
    private List<DobihRecord> _dobihRecords = new();

    public void Import()
    {
        ReadHillCsvZip();
        // WriteDobihRecordsToFile(records);
    }

    private void ReadHillCsvZip()
    {
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);

        using (var zipArchive = new ZipArchive(FileStreams.HillCsvZip))
        using (var csvStream = zipArchive.Entries[0].Open())
        using (var reader = new StreamReader(csvStream))
        using (var csv = new CsvReader(reader, csvConfiguration))
        {
            csv.Context.RegisterClassMap<DobihRecordMap>();

            _dobihRecords = csv.GetRecords<DobihRecord>()
                .Where(r => r.Countries.Contains("Scotland"))
                .OrderByDescending(r => r.Metres)
                .ToList();
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
        var json = JsonSerializer.Serialize(_dobihRecords, jsonOptions);
        File.WriteAllText(path, json);
    }
}