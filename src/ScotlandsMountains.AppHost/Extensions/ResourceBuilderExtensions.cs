using Aspire.Hosting.Azure;

namespace ScotlandsMountains.AppHost.Extensions;

internal static class ResourceBuilderExtensions
{
    public static IResourceBuilder<ProjectResource> WithSwaggerUrls(this IResourceBuilder<ProjectResource> builder)
    {
        return builder.WithUrls(context =>
         {
             foreach (var url in context.Urls)
             {
                 if (string.IsNullOrEmpty(url.DisplayText))
                 {
                     url.Url += "/swagger";

                     if (url?.Endpoint?.Scheme == "http")
                         url.DisplayLocation = UrlDisplayLocation.DetailsOnly;
                 }
             }
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

}
