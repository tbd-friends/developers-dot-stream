namespace Developers.Stream.Infrastructure.Twitch;

public class TwitchConfiguration
{
    public string ClientId { get; set; } = null!;
    public string Secret { get; set; } = null!;
    public string RedirectUri { get; set; } = null!;
}