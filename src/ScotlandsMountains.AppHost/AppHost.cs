using ScotlandsMountains.AppHost.Extensions;
using ScotlandsMountains.ServiceDefaults;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder
    .AddSqlServer("mssql")
    .WithHostPort(14330)
    .WithScotlandsMountainsDataBindMount()
    .AddDatabase(AspireConstants.Database);

var migration = builder
    .AddProject<Projects.ScotlandsMountains_MigrationService>("migration")
    .WithReference(sql)
    .WaitFor(sql);

var storage = builder
    .AddAzureStorage(AspireConstants.Storage)
    .RunAsEmulatorWithDefaultPorts()
    .AddBlobs(AspireConstants.Blobs);

var messaging = builder
    .AddAzureServiceBus(AspireConstants.ServiceBus)
    .RunAsEmulator();

var uploadTopic = messaging.AddServiceBusTopic(AspireConstants.FileUploadTopic);
var subscription = uploadTopic.AddServiceBusSubscription(AspireConstants.FileUploadSubscription);

var functions = builder
    .AddAzureFunctionsProject<Projects.ScotlandsMountains_FunctionApp>("functions")
    .WithReference(sql)
    .WithReference(messaging)
    .WithReference(storage)
    .WaitFor(messaging)
    .WaitFor(storage);

var api = builder
    .AddProject<Projects.ScotlandsMountains_Api>("api")
    .WithSwaggerUrls()
    .WithReference(sql)
    .WithReference(storage)
    .WaitFor(migration)
    .WithReference(messaging);


builder.Build().Run();
