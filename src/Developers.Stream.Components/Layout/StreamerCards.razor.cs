using Developers.Stream.Infrastructure.Contracts;
using Developers.Stream.Shared_Kernel.DataTransfer;
using Microsoft.AspNetCore.Components;

namespace Developers.Stream.Components.Layout;

public partial class StreamerCards(IStreamerQuery query) : ComponentBase
{
    private IEnumerable<StreamerDto> Streamers { get; set; } = new List<StreamerDto>();

    protected override async Task OnInitializedAsync()
    {
        Streamers = await query.GetStreamers(searchTerm: string.Empty, CancellationToken.None);
    }

    public async Task OnFilterChanged(string term, CancellationToken cancellationToken)
    {
        Streamers = await query.GetStreamers(term, cancellationToken);

        StateHasChanged();
    }
}