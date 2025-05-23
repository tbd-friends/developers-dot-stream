﻿namespace Developers.Stream.Domain;

public class Channel
{
    public int Id { get; set; }
    public int StreamerId { get; set; }
    public int PlatformId { get; set; }
    public string Name { get; set; } = null!;
    public string State { get; set; } = null!;
    public bool IsVerified { get; set; }

    public virtual Streamer Streamer { get; set; } = null!;
    public virtual Platform Platform { get; set; } = null!;
}