using ScotlandsMountains.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("mssql")
    .WithScotlandsMountainsDataBindMount();

var db = sqlServer.AddDatabase("ScotlandsMountains");

var api = builder
    .AddProject<Projects.ScotlandsMountains_Api>("api")
    .WithReference(db); ;

builder.Build().Run();
