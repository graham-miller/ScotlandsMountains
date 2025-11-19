using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScotlandsMountains.Domain.Values;

namespace ScotlandsMountains.Infrastructure.Database.Converters;

internal class MapScaleConverter : ValueConverter<MapScale, decimal>
{
    public MapScaleConverter()
        : base(
            v => v.Value,
            v => new MapScale(v))
    { }
}
