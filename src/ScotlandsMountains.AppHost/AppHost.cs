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

var storage = builder
    .AddAzureStorage("storage")
    .RunAsEmulatorWithDefaultPorts()
    .AddBlobs("blobs");

var messaging = builder
    .AddAzureServiceBus("messaging")
    .RunAsEmulator();

var uploadTopic = messaging.AddServiceBusTopic("file-upload-topic");
var subscription = uploadTopic.AddServiceBusSubscription("file-upload-sub");

var functions = builder
    .AddAzureFunctionsProject<Projects.ScotlandsMountains_FunctionApp>("functions")
    .WithReference(messaging)
    .WithReference(storage);

var api = builder
    .AddProject<Projects.ScotlandsMountains_Api>("api")
    .WithSwaggerUrls()
    .WithReference(sql)
    .WithReference(storage)
    .WaitFor(migration)
    .WithReference(messaging);


builder.Build().Run();
