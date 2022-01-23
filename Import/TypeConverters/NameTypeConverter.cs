namespace ScotlandsMountains.Import.TypeConverters;

public class NameTypeConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        return text.RemoveAliases();
    }
}