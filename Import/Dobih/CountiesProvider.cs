namespace ScotlandsMountains.Import.Dobih;

internal class CountiesProvider
{
    private readonly List<Provider> _providers;

    public CountiesProvider(IEnumerable<County> counties)
    {
        _providers = counties
            .Select(x => new Provider(x))
            .ToList();
    }

    public List<County> GetCounties(DobihRecord record)
    {
        return _providers
            .Where(x => x.IsInCounty(record))
            .Select(x => x.County)
            .OrderBy(x => x.Name)
            .ToList();
    }

    private class Provider
    {
        public Provider(County county)
        {
            County = county;
        }

        public County County { get; }

        public bool IsInCounty(DobihRecord record)
        {
            return record.County.SplitCounties().Contains(County.Name);
        }
    }
}