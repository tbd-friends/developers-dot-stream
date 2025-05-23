using Developers.Stream.Infrastructure;
using Developers.Stream.Infrastructure.Contexts;
using Developers.Stream.Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
namespace Developers.Stream.Streamer.Api.Configuration;

public static class DatabaseConfiguration
{
    public static IHostApplicationBuilder AddDatabase(this IHostApplicationBuilder builder)
    {
        builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(configure =>
            configure.UseNpgsql(builder.Configuration.GetConnectionString("developers-stream"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        builder.Services.AddTransient<ApplicationDbContext>(provider =>
            provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

        builder.Services.AddTransient(typeof(IRepository<>), typeof(ApplicationRepository<>));

        return builder;
    }
}