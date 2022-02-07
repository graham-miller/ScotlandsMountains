// See https://aka.ms/new-console-template for more information
using ScotlandsMountains.Import;

var reader = new HillCsvZipReader();
reader.Read();

var writer = new CosmosDbWriter();
writer.Write(reader);
