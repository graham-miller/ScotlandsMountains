using ScotlandsMountains.Application.UseCases.DobihFiles.Models;
using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Factories;

internal static class RegionsFactory
{
    public static List<Region> BuildFrom(DobihRecordsByNumber records)
    {
        return records.All
        .Select(line => line.Region)
        .Distinct()
        .OrderBy(region => region)
        .Select((region, index) =>
        {
            var split = region.Split(':', StringSplitOptions.TrimEntries);
            var code = split[0];
            var name = split[1];

            if (code == "08A") name = "Cairngorms North";
            if (code == "08B") name = "Cairngorms South";

            var displayOrder = index + 1;

            return new Region(code, name, displayOrder, region);
        })
        .ToList();
    }

}
