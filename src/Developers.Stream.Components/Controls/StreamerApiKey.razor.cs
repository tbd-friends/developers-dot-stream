using Developers.Stream.Infrastructure.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Developers.Stream.Components.Controls;

public partial class StreamerApiKey(
    AuthenticationStateProvider stateProvider,
    IStreamerProfileService profileService) : ComponentBase
{
    public string ApiKey { get; set; } = string.Empty;

    private async Task GenerateApiKey()
    {
        var userState = await stateProvider.GetAuthenticationStateAsync();

        var userId = Guid.Parse(userState.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        ApiKey = await profileService.GenerateApiKey(userId, CancellationToken.None);
    }
}