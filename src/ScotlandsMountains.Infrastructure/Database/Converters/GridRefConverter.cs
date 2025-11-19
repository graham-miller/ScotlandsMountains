using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScotlandsMountains.Domain.Values;

namespace ScotlandsMountains.Infrastructure.Database.Converters;

internal class GridRefConverter : ValueConverter<GridRef, string>
{
    public GridRefConverter()
        : base(
            v => v.Value,
            v => new GridRef(v))
    { }
}
