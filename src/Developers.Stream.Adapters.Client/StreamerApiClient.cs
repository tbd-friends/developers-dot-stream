using System.Net.Http.Json;
using System.Text.Json;
using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Shared_Kernel.DataTransfer;

namespace Developers.Stream.Adapters.Client;

public class StreamerApiClient(HttpClient client) :
    IStreamerQuery,
    IStreamerProfileService
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

    public Task<StreamerProfile> FetchProfile(Guid userIdentifier, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProfile(Guid userIdentifier, string name, string blurb, CancellationToken cancellationToken)
    {
        // Forwarding auth to provide user identifier on othe
        throw new NotImplementedException();
    }

    public async Task<string> FetchTwitchRegistrationLink(Guid userIdentifier, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> FetchYouTubeRegistrationLink(Guid userIdentifier, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}