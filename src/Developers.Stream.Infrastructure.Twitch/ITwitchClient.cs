using Ardalis.Result;

namespace Developers.Stream.Infrastructure.Twitch;

public interface ITwitchClient
{
    Task<Result<string>> FetchChannelNameUsingAuthenticationCode(string code);
}