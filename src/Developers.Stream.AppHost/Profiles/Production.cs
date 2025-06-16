namespace Developers.Stream.AppHost.Profiles;

public static class Production
{
    public static IDistributedApplicationBuilder BuildProduction(this IDistributedApplicationBuilder builder)
    {
        var developersStream = builder.AddConnectionString("developers-stream");
        var developersStreamAuth = builder.AddConnectionString("developers-stream-auth");
        
        var migrationService = builder.AddProject<Projects.Developers_Stream_MigrationService>("migration");
        
        var api = builder.AddProject<Projects.Developers_Stream_Api>("api")
            .WithReference(developersStream)
            .WithReference(developersStreamAuth)
            .WaitForCompletion(migrationService);

        var web = builder.AddProject<Projects.Developers_Stream_Web>("web")
            .WithReference(developersStream)
            .WithReference(developersStreamAuth)
            .WithReference(api)
            .WaitForCompletion(migrationService)
            .WithExternalHttpEndpoints();

        builder.AddProject<Projects.Developers_Stream_Streamer_Api>("streamer-api")
            .WithReference(developersStream)
            .WithReference(developersStreamAuth)
            .WaitFor(api)
            .WaitFor(web)
            .WaitForCompletion(migrationService)
            .WithExternalHttpEndpoints();

        return builder;
    }
}