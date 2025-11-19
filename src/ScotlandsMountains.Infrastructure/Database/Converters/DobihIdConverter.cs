using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScotlandsMountains.Domain.Values;

namespace ScotlandsMountains.Infrastructure.Database.Converters;

internal class DobihIdConverter : ValueConverter<DobihId, int>
{
    public DobihIdConverter()
        : base(
            v => v.Value,
            v => new DobihId(v))
    { }
}
