
using CsvHelper;
using CsvHelper.Configuration;
using System.IO.Compression;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Parsing;

internal interface IDobihFileReader
{
    DobihRecords Read(Stream stream);
}

internal class DobihFileReader : IDobihFileReader
{
    public DobihRecords Read(Stream stream)
    {
        using var zip = new ZipArchive(stream);

        var entry = zip.Entries.Single();

        using var unzipped = entry.Open();
        using var reader = new StreamReader(unzipped);
        using var csv = new CsvReader(reader, CsvConfiguration.FromAttributes<DobihRecord>());

        csv.Read();
        csv.ReadHeader();

        return new DobihRecords(
            entry.Name,
            csv.GetRecords<DobihRecord>()
                .OrderByDescending(line => line.Metres)
                .ToList());
    }
}

internal class DobihRecords(string fileName, List<DobihRecord> records) : List<DobihRecord>(records)
{
    public string FileName { get; init; } = fileName;
}
