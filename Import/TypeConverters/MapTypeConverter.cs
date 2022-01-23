namespace ScotlandsMountains.Import.TypeConverters;

public class MapTypeConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        return text.Split(' ').ToList();
    }
}