using ScotlandsMountains.AppHost.Extensions;
using ScotlandsMountains.ServiceDefaults;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder
    .AddSqlServer("sql")
    .WithHostPort(14330)
    .WithScotlandsMountainsDataBindMount()
    .AddDatabase(AspireConstants.Database)
    .WithMigrationsCommands();

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
    .AddAzureFunctionsProject<Projects.ScotlandsMountains_FunctionApp>("func")
    .WithReference(db).WaitFor(db)
    .WithReference(messaging).WaitFor(messaging)
    .WithReference(storage).WaitFor(storage);

var api = builder
    .AddProject<Projects.ScotlandsMountains_Api>("api")
    .WithSwaggerUrls()
    .WithReference(db).WaitFor(db)
    .WithReference(storage).WaitFor(storage)
    .WithReference(messaging).WaitFor(messaging);

builder.Build().Run();
