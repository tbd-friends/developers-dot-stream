var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgWeb();

var postgresdb = postgres.AddDatabase("developers-stream");
var authpostgresdb = postgres.AddDatabase("developers-stream-auth");

var migrationService = builder.AddProject<Projects.Developers_Stream_MigrationService>("migration")
    .WithReference(postgresdb)
    .WithReference(authpostgresdb)
    .WaitFor(postgresdb)
    .WaitFor(authpostgresdb);

builder.AddProject<Projects.Developers_Stream_Web>("web")
    .WithReference(postgresdb)
    .WaitForCompletion(migrationService);

builder.Build().Run();