using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Parsing;

internal interface ICountriesFactory
{
    List<Country> Build();
}

internal class CountriesFactory : ICountriesFactory
{
    public List<Country> Build()
    {
        return [
            new Country("Scotland", 0, 'S'),
            new Country("England", 1, 'E'),
            new Country("Wales", 2, 'W'),
            new Country("Ireland", 3, 'I'),
            new Country("Channel Islands", 4, 'C'),
            new Country("Isle of Man", 5, 'M')
        ];
    }
}
