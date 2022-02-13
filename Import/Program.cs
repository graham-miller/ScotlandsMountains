// See https://aka.ms/new-console-template for more information
using ScotlandsMountains.Import;

var config = Options.Create(new CosmosConfig());
var logger = LoggerFactory
    .Create(builder =>
    {
        builder.ClearProviders();
        builder.AddSimpleConsole(options =>
        {
            options.IncludeScopes = false;
            options.SingleLine = true;
            options.TimestampFormat = "hh:mm:ss";
            options.UseUtcTimestamp = true;
        });
        builder.SetMinimumLevel(LogLevel.Information);
    })
    .CreateLogger("Import");

var reader = new HillCsvZipReader(logger);
reader.Read();

var writer = new MountainDataWriter(config, logger);
await writer.Write(reader);
