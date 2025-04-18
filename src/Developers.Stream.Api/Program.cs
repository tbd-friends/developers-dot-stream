using Developers.Stream.Api.Configuration;
using Developers.Stream.Api.Infrastructure;
using Developers.Stream.Application.Commands;
using Developers.Stream.Application.Queries;
using Developers.Stream.Infrastructure.Twitch;
using Developers.Stream.Infrastructure.YouTube;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddServiceDefaults()
    .AddDatabase();

builder.Services.Configure<TwitchConfiguration>(builder.Configuration.GetSection("twitch"));
builder.Services.Configure<YouTubeConfiguration>(builder.Configuration.GetSection("youtube"));

builder.Services.AddHttpClient<ITwitchClient, TwitchClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["twitch:authUrl"]!);
});

builder.Services.AddTransient<IYouTubeClient, YouTubeClient>();

builder.Services
    .AddSingleton<TwitchChannelNameFromAuthenticationDelegate>(_ =>
        TwitchDefaults.FetchChannelFromAuthenticationResponse);

builder.Services.Configure<ServicesConfiguration>(builder.Configuration.GetSection("services"));

builder.Services.AddOpenApi();

builder.Services.AddMediator();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/streamers", async (ISender sender) => await sender.Send(new GetStreamers.Query()));
app.MapGet("/link-account", async (ISender sender,
    IOptions<ServicesConfiguration> configuration,
    string code,
    string state) =>
{
    await sender.Send(new LinkAccount.Command(code, state));

    return Results.Redirect(configuration.Value.ProfileRedirect);
});

app.Run();