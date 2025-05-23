using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Shared_Kernel.DataTransfer;
using Microsoft.AspNetCore.Components;

namespace Developers.Stream.Components.Layout;

public partial class StreamerCards(IStreamerQuery query) : ComponentBase
{
    private IEnumerable<StreamerDto> Streamers { get; set; } = new List<StreamerDto>();

    protected override async Task OnInitializedAsync()
    {
        Streamers = await query.GetStreamers(CancellationToken.None);
    }
}