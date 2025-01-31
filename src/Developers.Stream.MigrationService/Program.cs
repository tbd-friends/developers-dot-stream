using Developers.Stream.Domain;
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
        .UseSeeding((context, _) =>
        {
            context.Set<Platform>().AddRange(
                new Platform { Name = "Twitch" },
                new Platform { Name = "YouTube" },
                new Platform { Name = "Kick" }
            );
        });
#else
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("developers-stream"),
        x => x.MigrationsAssembly("Developers.Stream.Migrations"));
#endif
});

var host = builder.Build();

host.Run();