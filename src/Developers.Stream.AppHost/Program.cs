using System.Net.Sockets;

var builder = DistributedApplication.CreateBuilder(args);

var passwordParameter = builder.AddParameter("postgres-password", "safePwsAreBetter");

var postgres = builder.AddPostgres("postgres", password: passwordParameter, port: 15432)
    .WithDataVolume()
    .WithPgWeb();

var postgresdb = postgres.AddDatabase("developers-stream");
var authpostgresdb = postgres.AddDatabase("developers-stream-auth");

var migrationService = builder.AddProject<Projects.Developers_Stream_MigrationService>("migration")
    .WithReference(postgresdb)
    .WithReference(authpostgresdb)
    .WaitFor(postgresdb)
    .WaitFor(authpostgresdb);

var api = builder.AddProject<Projects.Developers_Stream_Api>("api")
    .WithReference(postgresdb)
    .WithReference(authpostgresdb)
    .WaitForCompletion(migrationService);

var web = builder.AddProject<Projects.Developers_Stream_Web>("web")
    .WithReference(postgresdb)
    .WithReference(authpostgresdb)
    .WithReference(api)
    .WaitForCompletion(migrationService);

builder.AddProject<Projects.Developers_Stream_Streamer_Api>("streamerApi")
    .WithReference(postgresdb)
    .WaitFor(api)
    .WaitFor(web)
    .WaitForCompletion(migrationService);

await builder.Build().RunAsync();