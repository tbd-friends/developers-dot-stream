using Developers.Stream.Streamer.Api.Configuration;
using Developers.Stream.Streamer.Api.Filters;

var builder = WebApplication
    .CreateBuilder(args);

builder
    .AddServiceDefaults()
    .AddDatabase();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/streamer/{username}", (string username) =>
    {
        return username;
    })
    .AddEndpointFilter<ApiKeyFilter>()
    .WithName("StreamerByName")
    .WithDescription("Search for a streamer by their username in your current platform");

await app.RunAsync();