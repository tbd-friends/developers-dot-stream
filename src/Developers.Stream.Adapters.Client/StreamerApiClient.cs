using System.Net.Http.Json;
using System.Text.Json;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Shared_Kernel.DataTransfer;

namespace Developers.Stream.Adapters.Client;

public class StreamerApiClient(HttpClient client) : IStreamerQuery
{
    public async Task<IEnumerable<StreamerDto>> GetStreamers(CancellationToken cancellationToken)
    {
        var request = await client.GetAsync("/streamers", cancellationToken);

        if (request.IsSuccessStatusCode)
        {
            return await request.Content.ReadFromJsonAsync<IEnumerable<StreamerDto>>(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }, cancellationToken) ?? [];
        }

        return [];
    }
}