namespace ScotlandsMountains.Import.TypeConverters;

public static class NameExtensions
{
    public static string RemoveAliases(this string name)
    {
        var result = name;
        var changed = false;

        do
        {
            changed = false;

            var start = result.IndexOf('[');
            if (start < 0) break;

            var end = result.IndexOf(']');
            if (end < 0) break;

            if (end > start)
            {
                result = result.Remove(start, end - start + 1);
                changed = true;
            }
        } while (changed);

        return result.Trim();
    }

    public static List<string> GetAliases(this string name)
    {
        var remaining = name;
        var result = new List<string>();
        var changed = false;

        do
        {
            changed = false;

            var start = remaining.IndexOf('[');
            if (start < 0) break;

            var end = remaining.IndexOf(']');
            if (end < 0) break;

            if (end > start)
            {
                result.Add(remaining.Substring(start + 1, end - start - 1).Trim());
                remaining = remaining.Remove(start, end - start + 1);
                changed = true;
            }
        } while (changed);

        return result.Select(s => s.Trim()).ToList();
    }
}