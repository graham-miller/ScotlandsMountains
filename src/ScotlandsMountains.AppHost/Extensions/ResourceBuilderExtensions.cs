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
                     url.DisplayText = $"Swagger UI ({url.Endpoint?.Scheme?.ToLower()})";
                     url.Url += "/swagger";

                     if (url?.Endpoint?.Scheme == "http")
                         url.DisplayLocation = UrlDisplayLocation.DetailsOnly;
                 }
             }
         });
    }
}
