namespace Developers.Stream.Shared_Kernel.DataTransfer;

public class StreamerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Blurb { get; set; } = null!;
    public IEnumerable<ChannelDto> Channels { get; set; } = null!;
}

public class ChannelDto
{
    public PlatformIdentifier Platform { get; set; } = null!;
    public string Name { get; set; } = null!;
}