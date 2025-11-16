using ScotlandsMountains.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder
    .AddSqlServer("mssql")
    .WithHostPort(14330)
    .WithScotlandsMountainsDataBindMount()
    .AddDatabase("ScotlandsMountains");

var migration = builder
    .AddProject<Projects.ScotlandsMountains_MigrationService>("migration")
    .WithReference(sql)
    .WaitFor(sql);

var api = builder
    .AddProject<Projects.ScotlandsMountains_Api>("api")
    .WithUrls(context =>
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
    })
    .WithReference(sql)
    .WaitFor(migration);

builder.Build().Run();
