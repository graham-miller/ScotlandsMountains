using System.Globalization;
using System.IO.Compression;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using ScotlandsMountains.Import.Files;

namespace ScotlandsMountains.Import
{
    public class HillCsvZipImporter
    {
        public void Import()
        {
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);

            using (var zipArchive = new ZipArchive(FileStreams.HillCsvZip))
            using (var csvStream = zipArchive.Entries[0].Open())
            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, csvConfiguration))
            {
                csv.Context.RegisterClassMap<DobihRecordMap>();
                var records = csv.GetRecords<DobihRecord>()
                    .Where(r => r.Countries.Contains("Scotland"))
                    .OrderByDescending(r => r.Metres)
                    .ToList();

                // WriteToFile(records);

            }
        }

        // ReSharper disable once UnusedMember.Local
        private static void WriteToFile(List<DobihRecord> records)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "mountain-records.json");
            var json = JsonSerializer.Serialize(records, jsonOptions);
            File.WriteAllText(path, json);
        }
    }
}