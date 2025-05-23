using System.Reflection;

namespace Developers.Stream.Shared_Kernel;

public class PlatformIdentifier {
    public static PlatformIdentifier Twitch = new("Twitch", "https://twitch.tv/");
    public static PlatformIdentifier YouTube = new("YouTube", "https://www.youtube.com/");
    public static PlatformIdentifier Kick = new("Kick", "https://www.kick.tv/");

    public string Identifier { get; }
    public string Address { get; }

    private static readonly PlatformIdentifier?[] Platforms = typeof(PlatformIdentifier)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.FieldType == typeof(PlatformIdentifier))
        .Select(f => f.GetValue(null) as PlatformIdentifier)
        .Where(f => f is not null)
        .ToArray();

    private PlatformIdentifier(string identifier, string siteAddress)
    {
        Identifier = identifier;
        Address = siteAddress;
    }

    public static implicit operator string(PlatformIdentifier platformIdentifier)
    {
        return platformIdentifier.Identifier;
    }

    public static implicit operator PlatformIdentifier(string identifier)
    {
        var platform = Platforms.FirstOrDefault(f => f!.Identifier == identifier);

        ArgumentNullException.ThrowIfNull(platform);

        return platform;
    }
}