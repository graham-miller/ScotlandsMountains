namespace ScotlandsMountains.Import.TypeConverters;

public class AliasesTypeConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        return text.GetAliases();
    }
}