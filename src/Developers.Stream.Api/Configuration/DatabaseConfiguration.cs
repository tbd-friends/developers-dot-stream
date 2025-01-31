using Developers.Stream.Infrastructure.Contexts;

namespace Developers.Stream.Api.Configuration;

public static class DatabaseConfiguration
{
    public static IHostApplicationBuilder AddDatabase(this IHostApplicationBuilder builder)
    {
        builder.Services.AddNpgsql<ApplicationDbContext>(
            builder.Configuration.GetConnectionString("developers.stream"));
        
        return builder;
    }
}