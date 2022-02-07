namespace ScotlandsMountains.Import.Dobih;

public class DobihRecordMap : ClassMap<DobihRecord>
{
    public DobihRecordMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.Name).Name("Name").TypeConverter<NameTypeConverter>();
        Map(m => m.Aliases).Name("Name").TypeConverter<AliasesTypeConverter>();
        Map(m => m.Countries).Name("Country").TypeConverter<CountryTypeConverter>();
        Map(m => m.Classifications).Name("Classification").TypeConverter<ClassificationTypeConverter>();
        Map(m => m.Maps1To50K).Name("Map 1:50k").TypeConverter<MapTypeConverter>();
        Map(m => m.Maps1To25K).Name("Map 1:25k").TypeConverter<MapTypeConverter>();
        Map(m => m.IsMemberOf).Index(0).TypeConverter<IsMemberOfTypeConverter>();
    }
}