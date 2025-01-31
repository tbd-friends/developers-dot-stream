using Developers.Stream.Adapters.Client;
using Developers.Stream.Infrastructure.Contracts;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient<StreamerApiClient>(configure =>
{
    configure.BaseAddress = new Uri("http+https://web");
});

builder.Services.AddScoped<IStreamerQuery, StreamerApiClient>();

await builder.Build().RunAsync();