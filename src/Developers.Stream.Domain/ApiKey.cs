namespace Developers.Stream.Domain;

public class ApiKey
{
    public int Id { get; set; }
    public int StreamerId { get; set; }
    public string Key { get; set; } = null!;
    public DateTime DateAdded { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual Streamer Streamer { get; set; } = null!;
}