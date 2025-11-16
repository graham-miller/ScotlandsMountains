var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ScotlandsMountains_Api>("api");

builder.Build().Run();
