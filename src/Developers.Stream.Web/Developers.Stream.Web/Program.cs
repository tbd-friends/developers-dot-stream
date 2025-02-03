using Developers.Stream.Adapters.Server;
using Developers.Stream.Infrastructure;
using Developers.Stream.Infrastructure.Contexts;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Web.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.AddNpgsqlDbContext<ApplicationDbContext>(connectionName: "developers-stream");

builder.Services.AddTransient<ApplicationDbContext>(provider =>
    provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

builder.Services.AddTransient(typeof(IRepository<>), typeof(ApplicationRepository<>));
builder.Services.AddTransient<IStreamerQuery, StreamerQueryService>();

builder.Services.AddMediator();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Developers.Stream.Web.Client._Imports).Assembly);

app.Run();