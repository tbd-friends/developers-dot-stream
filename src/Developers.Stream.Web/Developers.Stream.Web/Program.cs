using Developers.Stream.Infrastructure.Auth;
using Developers.Stream.Infrastructure.Twitch;
using Developers.Stream.Web.Components;
using Developers.Stream.Web.Components.Account;
using Developers.Stream.Web.Configuration;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddServiceDefaults()
    .AddDatabase()
    .AddAuth();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddMediator();

builder.Services.Configure<TwitchConfiguration>(builder.Configuration.GetSection("twitch"));

builder.Services.AddHttpClient<ITwitchClient, TwitchClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["twitch:authUrl"]!);
});

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Developers.Stream.Web.Client._Imports).Assembly);

app.MapAdditionalIdentityEndpoints();

app.Run();