namespace Developers.Stream.Domain;

public class Streamer
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Blurb { get; set; } = null!;

    public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}