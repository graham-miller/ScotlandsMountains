using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScotlandsMountains.Application;
using ScotlandsMountains.Infrastructure;
using ScotlandsMountains.Infrastructure.Database;
using ScotlandsMountains.ServiceDefaults;

var builder = FunctionsApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddSqlServerDbContext<ScotlandsMountainsDbContext>(AspireConstants.Database);
builder.AddAzureBlobServiceClient(AspireConstants.Blobs);
builder.AddAzureServiceBusClient(AspireConstants.ServiceBus);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    .AddApplication()
    .AddInfrastructure();

builder.Build().Run();
