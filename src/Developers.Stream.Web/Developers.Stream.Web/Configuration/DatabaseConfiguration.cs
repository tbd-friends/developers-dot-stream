using Developers.Stream.Adapters.Server;
using Developers.Stream.Infrastructure;
using Developers.Stream.Infrastructure.Contexts;
using Developers.Stream.Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Developers.Stream.Web.Configuration;

public static class DatabaseConfiguration
{
    public static IHostApplicationBuilder AddDatabase(this IHostApplicationBuilder builder)
    {
        builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(configure =>
            configure.UseNpgsql(builder.Configuration.GetConnectionString("developers-stream")));

        builder.Services.AddTransient(typeof(IRepository<>), typeof(ApplicationRepository<>));
        builder.Services.AddTransient<IStreamerQuery, StreamerQueryService>();
        builder.Services.AddTransient<ApplicationDbContext>(provider =>
            provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

        return builder;
    }
}