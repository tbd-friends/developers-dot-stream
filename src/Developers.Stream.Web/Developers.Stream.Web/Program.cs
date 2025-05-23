using Developers.Stream.Adapters.Server;
using Developers.Stream.Infrastructure.Auth;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Infrastructure.Kick;
using Developers.Stream.Infrastructure.Twitch;
using Developers.Stream.Infrastructure.YouTube;
using Developers.Stream.Web.Components;
using Developers.Stream.Web.Components.Account;
using Developers.Stream.Web.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.FeatureManagement;

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

builder.Services.AddFeatureManagement();

builder.Services.AddAntDesign();

builder.Services.AddMediator();

builder.Services.Configure<KickConfiguration>(builder.Configuration.GetSection("kick"));
builder.Services.Configure<TwitchConfiguration>(builder.Configuration.GetSection("twitch"));
builder.Services.Configure<YouTubeConfiguration>(builder.Configuration.GetSection("youtube"));

builder.Services.AddHttpClient<IKickClient, KickClient>(client => {
    client.BaseAddress = new Uri(builder.Configuration["kick:authUrl"]!);
});

builder.Services.AddHttpClient<ITwitchClient, TwitchClient>(client => {
    client.BaseAddress = new Uri(builder.Configuration["twitch:authUrl"]!);
});

builder.Services.AddTransient<IYouTubeClient, YouTubeClient>();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddTransient<IStreamerProfileService, StreamerProfileService>();

builder.Services
    .AddSingleton<KickChannelNameFromAuthenticationDelegate>(_ =>
        KickDefaults.FetchChannelFromAuthenticationResponse);

builder.Services
    .AddSingleton<TwitchChannelNameFromAuthenticationDelegate>(_ =>
        TwitchDefaults.FetchChannelFromAuthenticationResponse);

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

app.UseAuthentication();

app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Developers.Stream.Web.Client._Imports).Assembly);

app.MapAdditionalIdentityEndpoints();

app.Run();