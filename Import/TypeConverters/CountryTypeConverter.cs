namespace ScotlandsMountains.Import.TypeConverters;

public class CountryTypeConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        switch (text)
        {
            case "C":
                return new List<string> { "Channel Islands" };
            case "E":
                return new List<string> { "England" };
            case "ES":
                return new List<string> { "England", "Scotland" };
            case "I":
                return new List<string> { "Ireland" };
            case "M":
                return new List<string> { "Isle of Man" };
            case "S":
                return new List<string> { "Scotland" };
            case "W":
                return new List<string> { "Wales" };
            default:
                throw new ArgumentOutOfRangeException($"'{text}' is not a known country code");
        }
    }
}