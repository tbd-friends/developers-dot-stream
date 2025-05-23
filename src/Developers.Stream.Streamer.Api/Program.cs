using Ardalis.Result.AspNetCore;
using Developers.Stream.Streamer.Api.Configuration;
using Developers.Stream.Streamer.Api.Filters;
using Developers.Stream.Streamer.Application;
using Mediator;

var builder = WebApplication
    .CreateBuilder(args);

builder
    .AddServiceDefaults()
    .AddDatabase();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMediator();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/streamer/{username}", async (string username, ISender sender) =>
        (await sender.Send(new StreamerByName.Query(username), CancellationToken.None)).ToMinimalApiResult())
    .AddEndpointFilter<ApiKeyFilter>()
    .WithName("StreamerByName")
    .WithDescription("Search for a streamer by their username in your current platform");

await app.RunAsync();