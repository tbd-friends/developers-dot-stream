using Developers.Stream.Application.Queries;
using Mediator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddMediator();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/streamers", async (ISender sender) => await sender.Send(new GetLiveStreamers.Query()));

app.Run();