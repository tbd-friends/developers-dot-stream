using System.Reflection;

namespace Developers.Stream.Shared_Kernel;

public class PlatformIdentifier
{
    public static PlatformIdentifier Twitch = new("Twitch");
    public static PlatformIdentifier YouTube = new("YouTube");
    public static PlatformIdentifier Kick = new("Kick");

    public string Identifier { get; }

    public static readonly PlatformIdentifier?[] Platforms = typeof(PlatformIdentifier)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.FieldType == typeof(PlatformIdentifier))
        .Select(f => f.GetValue(null) as PlatformIdentifier)
        .Where(f => f is not null) 
        .ToArray();

    private PlatformIdentifier(string identifier)
    {
        Identifier = identifier;
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