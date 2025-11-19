using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScotlandsMountains.Domain.Values;

namespace ScotlandsMountains.Infrastructure.Database.Converters;

internal class CoordinatesConverter : ValueConverter<Coordinates, string>
{
    public CoordinatesConverter() : base(
        v => $"{v.Latitude},{v.Longitude}",
        v => new Coordinates(v))
    { }
}