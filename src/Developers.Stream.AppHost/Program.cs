using Developers.Stream.AppHost.Profiles;

var builder = DistributedApplication.CreateBuilder(args);

if (builder.ExecutionContext.IsRunMode)
{
    builder.BuildDevelopment();
}
else
{
    builder.BuildProduction();
}

await builder.Build().RunAsync();