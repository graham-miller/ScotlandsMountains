using System.Globalization;
using System.IO.Compression;
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
                var records = csv.GetRecords<DobihRecord>().ToList();
            }
        }
    }
}