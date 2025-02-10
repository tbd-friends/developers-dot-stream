using Developers.Stream.Infrastructure.Auth.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Developers.Stream.MigrationService;

public class InitializeAuthContextWorker(
    IDbContextFactory<AuthDbContext> factory,
    ILogger<InitializeAuthContextWorker> logger,
    IHostApplicationLifetime applicationLifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RunMigrationAsync(stoppingToken);

        applicationLifetime.StopApplication();
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