using Aspire.Hosting.Azure;

namespace ScotlandsMountains.AppHost.Extensions;

internal static class ResourceBuilderExtensions
{
    public static IResourceBuilder<ProjectResource> WithSwaggerUrls(this IResourceBuilder<ProjectResource> builder)
    {
        return builder.WithUrls(context =>
        {
            context.Urls.ForEach(url =>
            {
                if (string.IsNullOrEmpty(url.DisplayText))
                {
                    url.Url += "/swagger";
    
                    if (url?.Endpoint?.Scheme == "http")
                        url.DisplayLocation = UrlDisplayLocation.DetailsOnly;
                    }
            });
        });
    }

    internal static IResourceBuilder<AzureStorageResource> RunAsEmulatorWithDefaultPorts(this IResourceBuilder<AzureStorageResource> builder)
    {
        builder.RunAsEmulator(azurite =>
        {
            azurite.WithEndpoint("blob", e => e.Port = 10000);
            azurite.WithEndpoint("queue", e => e.Port = 10001);
            azurite.WithEndpoint("table", e => e.Port = 10002);
        });

        return builder;
    }

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
