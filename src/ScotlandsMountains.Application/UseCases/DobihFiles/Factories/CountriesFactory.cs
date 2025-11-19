using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Factories;

internal class CountriesFactory
{
    internal static IEnumerable<Country> Build()
    {
        yield return new Country("Scotland", 0, 'S');
        yield return new Country("England", 1, 'E');
        yield return new Country("Wales", 2, 'W');
        yield return new Country("Ireland", 3, 'I');
        yield return new Country("Channel Islands", 4, 'C');
        yield return new Country("Isle of Man", 5, 'M');
    }
}
