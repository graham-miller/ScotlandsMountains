
using CsvHelper;
using CsvHelper.Configuration;
using System.IO.Compression;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Models;

internal class DobihFile
{
    private readonly Dictionary<int, DobihRecord> _recordsByNumber = new Dictionary<int, DobihRecord>();

    public DobihFile(Stream stream)
    {
        using var zip = new ZipArchive(stream);
        
        var entry = zip.Entries.Single();
        FileName = entry.Name;

        using var unzipped = entry.Open();
        using var reader = new StreamReader(unzipped);
        using var csv = new CsvReader(reader, CsvConfiguration.FromAttributes<DobihRecord>());

        csv.Read();
        csv.ReadHeader();

        _recordsByNumber = csv.GetRecords<DobihRecord>()
            .OrderByDescending(line => line.Metres)
            .ToDictionary(r => r.Number, r => r);
    }

    public string FileName { get; init; }

    public IEnumerable<DobihRecord> Records => _recordsByNumber.Values;

    public bool TryGetRecord(int number, out DobihRecord? record)
    {
        return _recordsByNumber.TryGetValue(number, out record);
    }
}
