using Microsoft.AspNetCore.Components;

namespace Developers.Stream.Components;

public partial class StreamerCard : ComponentBase
{
    [Parameter] public bool IsLive { get; set; }
    [Parameter] public string StreamerName { get; set; } = null!;
    [Parameter] public string Description { get; set; } = null!;
    [Parameter] public IEnumerable<string> Badges { get; set; } = new List<string>();

    private string OnlineStatus => IsLive ? string.Empty : "offline";
}