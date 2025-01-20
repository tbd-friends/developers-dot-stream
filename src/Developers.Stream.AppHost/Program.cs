var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgWeb();

var postgresdb = postgres.AddDatabase("developers.stream");

builder.AddProject<Projects.Developers_Stream_Web>("web")
    .WithReference(postgresdb);

builder.Build().Run();