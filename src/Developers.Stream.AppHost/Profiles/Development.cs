namespace Developers.Stream.AppHost.Profiles;

public static class Development // Enter a value for the 'postgres_password' infrastructure parameter:
{
    public static IDistributedApplicationBuilder BuildDevelopment(this IDistributedApplicationBuilder builder)
    {
        var postgres = builder.AddPostgres("postgres", port: 15432)
            .WithDataVolume()
            .WithPgWeb()
            .PublishAsContainer();

        var developersStreamDb = postgres.AddDatabase("developers-stream");
        var developersStreamAuthDb = postgres.AddDatabase("developers-stream-auth");

        var migrationService = builder.AddProject<Projects.Developers_Stream_MigrationService>("migration")
            .WithReference(developersStreamDb)
            .WithReference(developersStreamAuthDb)
            .WaitFor(developersStreamDb)
            .WaitFor(developersStreamAuthDb);

        var api = builder.AddProject<Projects.Developers_Stream_Api>("api")
            .WithReference(developersStreamDb)
            .WithReference(developersStreamAuthDb)
            .WaitForCompletion(migrationService);

        var web = builder.AddProject<Projects.Developers_Stream_Web>("web")
            .WithReference(developersStreamDb)
            .WithReference(developersStreamAuthDb)
            .WithReference(api)
            .WaitForCompletion(migrationService)
            .WithExternalHttpEndpoints();

        builder.AddProject<Projects.Developers_Stream_Streamer_Api>("streamer-api")
            .WithReference(developersStreamDb)
            .WaitFor(api)
            .WaitFor(web)
            .WaitForCompletion(migrationService)
            .WithExternalHttpEndpoints();

        return builder;
    }
}