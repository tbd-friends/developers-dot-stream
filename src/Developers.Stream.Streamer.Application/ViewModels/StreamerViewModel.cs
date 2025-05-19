namespace Developers.Stream.Streamer.Application.ViewModels;

public class StreamerViewModel
{
    public string Name { get; set; } = null!;
    public string Blurb { get; set; } = null!;

    public IEnumerable<Channel> Channels { get; set; } = null!;
    public IEnumerable<string> Tags { get; set; } = null!;
    
    public class Channel
    {
        public string Platform { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}