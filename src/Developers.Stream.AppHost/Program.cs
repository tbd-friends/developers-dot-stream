var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Developers_Stream_Web>("web");

builder.Build().Run();