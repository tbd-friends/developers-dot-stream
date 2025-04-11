using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Developers.Stream.Web.Infrastructure;

public static class AuthenticationStateProviderExtensions
{
    public static Guid GetUserId(this AuthenticationState state) =>
        Guid.Parse(state.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
}