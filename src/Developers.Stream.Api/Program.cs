using Developers.Stream.Api.Configuration;
using Developers.Stream.Application.Commands;
using Developers.Stream.Application.Queries;
using Developers.Stream.Infrastructure.Twitch;
using Mediator;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddServiceDefaults()
    .AddDatabase();

builder.Services.Configure<TwitchConfiguration>(builder.Configuration.GetSection("twitch"));

builder.Services.AddHttpClient<ITwitchClient, TwitchClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["twitch:authUrl"]!);
});

builder.Services.AddOpenApi();

builder.Services.AddMediator();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/streamers", async (ISender sender) => await sender.Send(new GetLiveStreamers.Query()));
app.MapGet("/link-account", async (ISender sender, string code) => await sender.Send(new LinkTwitchAccount.Command(code)));

app.Run();