namespace ScotlandsMountains.AppHost.Extensions;

internal static class SqlServerBuilderExtensions
{
    public static IResourceBuilder<SqlServerServerResource> WithScotlandsMountainsDataBindMount(this IResourceBuilder<SqlServerServerResource> builder)
    {
        var path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".scotlandsmountains",
            "mssql-data");

        Directory.CreateDirectory(path);

        return builder.WithDataBindMount(source: path);
    }
}
