using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Application.UseCases.DobihFiles.Parsing;

internal interface IDobihRecordsParserFactory
{
    IDobihRecordsParser Build();
}

internal interface IDobihRecordsParser
{
    DobihRecordsParser.Output Parse(DobihRecords records);
}

internal class DobihRecordsParserFactory : IDobihRecordsParserFactory
{
    public IDobihRecordsParser Build()
    {
        return new DobihRecordsParser(
            new RegionsFactory(),
            new MapsFactory(),
            new ClassificationsFactory(),
            new CountiesFactory(),
            new CountriesFactory(),
            new MountainsFactory());
    }
}

internal class DobihRecordsParser : IDobihRecordsParser
{
    private readonly IRegionsFactory _regionsFactory;
    private readonly IMapsFactory _mapsFactory;
    private readonly IClassificationsFactory _classificationsFactory;
    private readonly ICountiesFactory _countiesFactory;
    private readonly ICountriesFactory _countriesFactory;
    private readonly IMountainsFactory _mountainsFactory;

    public DobihRecordsParser(
            IRegionsFactory regionsFactory,
            IMapsFactory mapsFactory,
            IClassificationsFactory classificationsFactory,
            ICountiesFactory countiesFactory,
            ICountriesFactory countriesFactory,
            IMountainsFactory mountainsFactory
        )
    {
        _regionsFactory = regionsFactory;
        _mapsFactory = mapsFactory;
        _classificationsFactory = classificationsFactory;
        _countiesFactory = countiesFactory;
        _countriesFactory = countriesFactory;
        _mountainsFactory = mountainsFactory;
    }

    public Output Parse(DobihRecords records)
    {
        var regions = _regionsFactory.BuildFrom(records);
        var maps = _mapsFactory.Build();
        var classifications = _classificationsFactory.Build();
        var counties = _countiesFactory.BuildFrom(records);
        var countries = _countriesFactory.Build();
        var mountains = _mountainsFactory.BuildFrom(records, regions, maps, classifications, counties, countries);

        return new Output(regions, maps, classifications, counties, countries, mountains);
    }

    public record Output(
        List<Region> Regions,
        List<Map> Maps,
        List<Classification> Classifications,
        List<County> Counties,
        List<Country> Countries,
        List<Mountain> Mountains);
}

