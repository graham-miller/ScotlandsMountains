using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScotlandsMountains.Domain.Values;

namespace ScotlandsMountains.Infrastructure.Database.Converters;

internal class HeightConverter : ValueConverter<Height, decimal>
{
    public HeightConverter()
        : base(
            v => v.Metres,
            v => new Height(v))
    { }
}
