namespace ScotlandsMountains.Import.TypeConverters;

public class IsMemberOfTypeConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        var isMemberOf = new IsMemberOf();

        foreach (var property in typeof(IsMemberOf).GetProperties())
        {
            var nameAttribute = (NameAttribute) Attribute.GetCustomAttribute(property, typeof(NameAttribute));
            if (nameAttribute != null)
            {
                var code = nameAttribute.Names[0];
                var index = Array.IndexOf(row.HeaderRecord, code);
                if (row.Parser.Record[index] == "1")
                    property.SetValue(isMemberOf, true);
            }
        }

        return isMemberOf;
    }
}