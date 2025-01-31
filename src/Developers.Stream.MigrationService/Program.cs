using Developers.Stream.Infrastructure.Contexts;
using Developers.Stream.MigrationService;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InitializeDbContextWorker>();

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("developers-stream"), 
        x => x.MigrationsAssembly("Developers.Stream.Migrations"));
});

var host = builder.Build();

host.Run();