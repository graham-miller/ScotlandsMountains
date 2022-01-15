using System.Globalization;
using System.IO.Compression;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
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

    public class DobihRecordMap : ClassMap<DobihRecord>
    {
        public DobihRecordMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Countries).Name("Country").TypeConverter<CountryTypeConverter>();
            Map(m => m.Classifications).Name("Classification").TypeConverter<ClassificationTypeConverter>();
            Map(m => m.Maps1To50K).Name("Map 1:50k").TypeConverter<MapTypeConverter>();
            Map(m => m.Maps1To25K).Name("Map 1:25k").TypeConverter<MapTypeConverter>();
            Map(m => m.IsMemberOf).Index(0).TypeConverter<IsMemberOfTypeConverter>();
        }
    }

    public class CountryTypeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            switch (text)
            {
                case "C":
                    return new List<string> { "Channel Islands" };
                case "E":
                    return new List<string> { "England" };
                case "ES":
                    return new List<string> { "England", "Scotland" };
                case "I":
                    return new List<string> { "Ireland" };
                case "M":
                    return new List<string> { "Isle of Man" };
                case "S":
                    return new List<string> { "Scotland" };
                case "W":
                    return new List<string> { "Wales" };
                default:
                    throw new ArgumentOutOfRangeException($"'{text}' is not a known country code");
            }
        }
    }

    public class ClassificationTypeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return text.Split(',').ToList();
        }
    }

    public class MapTypeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return text.Split(' ').ToList();
        }
    }

    public class IsMemberOfTypeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            var isMemberOf = new IsMemberOf();

            foreach (var property in typeof(IsMemberOf).GetProperties())
            {
                var nameAttribute = (NameAttribute) Attribute.GetCustomAttribute(property, typeof(NameAttribute));
                if (nameAttribute != null)
                {
                    var code = nameAttribute.Names[0];
                    var index = Array.IndexOf(row.HeaderRecord, code);
                    if (row.Parser.Record[index] == "1")
                        property.SetValue(isMemberOf, true);
                }
            }

            return isMemberOf;
        }
    }
}