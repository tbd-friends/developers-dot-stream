using Developers.Stream.Domain;
using Microsoft.EntityFrameworkCore;

namespace Developers.Stream.Infrastructure.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();
    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<Label> Labels => Set<Label>();
    public DbSet<Platform> Platforms => Set<Platform>();
    public DbSet<Streamer> Streamers => Set<Streamer>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}