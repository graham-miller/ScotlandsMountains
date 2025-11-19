using CsvHelper;
using ScotlandsMountains.Domain.Entities;
using System.Globalization;
using System.Reflection;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Factories;

internal static class ClassificationsFactory
{
    internal static IEnumerable<Classification> Build()
    {
        var name = $"{typeof(ClassificationsFactory).Namespace}.Resources.Classifications.csv";

        using var file =
            Assembly.GetExecutingAssembly().GetManifestResourceStream(name)
            ?? throw new Exception("Classifications.csv not found");

        using var reader = new StreamReader(file);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Read();
        csv.ReadHeader();

        while (csv.Read())
        {
            var info = csv.GetRecord<ClassificationInfo>();
            
            yield return info.ToClassification();
        }
    }

    internal class ClassificationInfo
    {
        public string DobihCode { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string NameSingular { get; set; } = null!;

        public int DisplayOrder { get; set; }

        public string Description { get; set; } = null!;

        public Classification ToClassification()
        {
            return new Classification(Name, NameSingular, DisplayOrder, Description, DobihCode);
        }
    }
}
