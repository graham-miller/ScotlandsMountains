using ScotlandsMountains.Application;
using ScotlandsMountains.Infrastructure;
using ScotlandsMountains.Infrastructure.Database;
using ScotlandsMountains.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Configure distributed application services
builder.AddServiceDefaults();
builder.AddSqlServerDbContext<ScotlandsMountainsDbContext>(AspireConstants.Database);
builder.AddAzureBlobServiceClient(AspireConstants.Blobs);
builder.AddAzureServiceBusClient(AspireConstants.ServiceBus);

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
