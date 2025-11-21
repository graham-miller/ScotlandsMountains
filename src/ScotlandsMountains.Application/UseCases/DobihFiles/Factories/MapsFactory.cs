using ScotlandsMountains.Domain.Entities;
using System.Reflection;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Factories;

internal static class MapsFactory
{
    internal static List<Map> Build()
    {
        var publisher = new MapPublisher(MapPublisher.OrdnanceSurvey);

        var explorerSeries = new MapSeries(MapSeries.Explorer, publisher, 1 / 25_000m);

        var landrangerSeries = new MapSeries(MapSeries.Landranger, publisher, 1 / 50_000m);

        var explorerMaps = BuildSeries(explorerSeries);
        var landrangerMaps = BuildSeries(landrangerSeries);

        return explorerMaps.Concat(landrangerMaps).ToList();
    }

    private static IEnumerable<Map> BuildSeries(MapSeries series)
    {
        var activeIsbns = CombineToLinePerMap(GetLinesFrom($"{series.Name}Active.txt"))
            .Select(ParseLine)
            .ToDictionary(x => x.code, x => x.isbn);

        foreach (var line in CombineToLinePerMap(GetLinesFrom($"{series.Name}.txt")))
        {
            var (code, name, isbn) = ParseLine(line);

            yield return new Map(code, name, isbn, activeIsbns[code], series);
        }
    }

    private static IEnumerable<string> GetLinesFrom(string name)
    {
        var path = $"{typeof(MapsFactory).Namespace}.Resources.{name}";

        using var file =
            Assembly.GetExecutingAssembly().GetManifestResourceStream(path)
            ?? throw new Exception($"{name} not found");

        using var reader = new StreamReader(file);

        while (!reader.EndOfStream)
        {
            yield return reader.ReadLine()!;
        }
    }

    private static List<string> CombineToLinePerMap(IEnumerable<string> splitLines)
    {
        var lines = new List<string>();
        var partialLine = string.Empty;

        foreach (var line in splitLines)
        {
            partialLine += string.IsNullOrEmpty(partialLine) ? string.Empty : " ";
            partialLine += line.Trim();

            if (EndsWithIsbn(partialLine))
            {
                lines.Add(partialLine.Trim());
                partialLine = string.Empty;
            }
        }

        return lines;
    }

    private static bool EndsWithIsbn(string s)
    {
        return
            s.Length >= 14
            && s.Substring(s.Length - 14, 1) == " "
            && s.Substring(s.Length - 13, 1).All(char.IsNumber);
    }

    private static (string code, string name, string isbn) ParseLine(string s)
    {
        const StringComparison sc = StringComparison.InvariantCulture;

        var firstSpace = s.IndexOf(" ", sc);
        var lastSpace = s.LastIndexOf(" ", sc);

        return (
            s.Substring(0, firstSpace),
            s.Substring(firstSpace + 1, lastSpace - firstSpace),
            s.Substring(lastSpace + 1)
        );
    }
}
