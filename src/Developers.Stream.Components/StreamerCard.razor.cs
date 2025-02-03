using Developers.Stream.Shared_Kernel.DataTransfer;
using Microsoft.AspNetCore.Components;

namespace Developers.Stream.Components;

public partial class StreamerCard : ComponentBase
{
    [Parameter] public StreamerDto Streamer { get; set; } = null!;

}