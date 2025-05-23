using Developers.Stream.Domain;
using Developers.Stream.Infrastructure.Auth.Contexts;
using Developers.Stream.Infrastructure.Contexts;
using Developers.Stream.MigrationService;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InitializeDbContextWorker>();

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
{
#if DEBUG
    options.UseNpgsql(
            builder.Configuration.GetConnectionString("developers-stream"),
            x => x.MigrationsAssembly("Developers.Stream.Migrations"))
        .UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            if (!context.Set<Platform>().Any())
            {
                await context.Set<Platform>().AddRangeAsync(
                    new Platform { Name = "Twitch" },
                    new Platform { Name = "YouTube" },
                    new Platform { Name = "Kick" }
                );

                await context.SaveChangesAsync(cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);
        });
#else
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("developers-stream"),
        x => x.MigrationsAssembly("Developers.Stream.Migrations"));
#endif
});

builder.Services.AddPooledDbContextFactory<AuthDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("developers-stream-auth"),
        x => x.MigrationsAssembly("Developers.Stream.Migrations"));
});

var host = builder.Build();

await host.RunAsync();