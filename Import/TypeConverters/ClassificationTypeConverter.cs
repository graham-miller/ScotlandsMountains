namespace ScotlandsMountains.Import.TypeConverters;

public class ClassificationTypeConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        return text.Split(',').ToList();
    }
}