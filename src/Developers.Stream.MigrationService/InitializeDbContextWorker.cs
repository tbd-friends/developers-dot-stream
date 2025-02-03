using Developers.Stream.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Developers.Stream.MigrationService;

public class InitializeDbContextWorker(
    IDbContextFactory<ApplicationDbContext> factory,
    ILogger<InitializeDbContextWorker> logger,
    IHostApplicationLifetime applicationLifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //await EnsureDatabaseAsync(context, stoppingToken);

        await RunMigrationAsync(stoppingToken);

        applicationLifetime.StopApplication();
    }

    private async Task EnsureDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            logger.LogInformation("Ensuring database exists");

            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                logger.LogInformation("Creating database");

                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private async Task RunMigrationAsync(CancellationToken cancellationToken)
    {
        await using var dbContext = await factory.CreateDbContextAsync(cancellationToken);

        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            logger.LogInformation("Applying migrations");

            await dbContext.Database.MigrateAsync(cancellationToken);

            logger.LogInformation("Migrations applied");
        });
    }
}