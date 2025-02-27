namespace Developers.Stream.Infrastructure.Twitch;

public interface ITwitchClient
{
    Task<TwitchAuthenticationResponse> FetchAuthenticationFromCode(string code);
}