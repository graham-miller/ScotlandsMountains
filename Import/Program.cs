// See https://aka.ms/new-console-template for more information
using ScotlandsMountains.Import;

Console.WriteLine("Reading HillCsv.zip...");
var reader = new HillCsvZipReader();
reader.Read();

var writer = new CosmosDbWriter();
await writer.Write(reader);
