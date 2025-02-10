using Developers.Stream.Infrastructure.Auth.Contexts;
using Developers.Stream.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Developers.Stream.MigrationService;

public class InitializeDbContextWorker(
    IDbContextFactory<ApplicationDbContext> factory,
    IDbContextFactory<AuthDbContext> authFactory,
    ILogger<InitializeDbContextWorker> logger,
    IHostApplicationLifetime applicationLifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RunStandardMigration(stoppingToken);

        await RunAuthMigration(stoppingToken);

        applicationLifetime.StopApplication();
    }

    private async Task RunStandardMigration(CancellationToken cancellationToken)
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

    private async Task RunAuthMigration(CancellationToken cancellationToken)
    {
        await using var dbContext = await authFactory.CreateDbContextAsync(cancellationToken);

        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            logger.LogInformation("Applying migrations");

            await dbContext.Database.MigrateAsync(cancellationToken);

            logger.LogInformation("Migrations applied");
        });
    }
}