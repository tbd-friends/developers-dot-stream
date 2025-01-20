namespace Developers.Stream.Domain;

public class Tag
{
    public int Id { get; set; }
    public int LabelId { get; set; }
    public int StreamerId { get; set; }

    public virtual Label Label { get; set; } = null!;
    public virtual Streamer Streamer { get; set; } = null!;
}