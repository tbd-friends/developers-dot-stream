namespace Developers.Stream.Infrastructure.YouTube;

public class YouTubeConfiguration
{
    public string AuthUrl { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string Secret { get; set; } = null!;
    public string RedirectUri { get; set; } = null!;
    public string CodeFlowRedirectUri { get; set; } = null!;
}