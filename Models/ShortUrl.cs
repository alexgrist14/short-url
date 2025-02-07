public class ShortUrl
{
    public int Id { get; set; }
    public required string OriginalUrl { get; set; }
    public required string ShortenedUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int Clicks { get; set; } = 0;
}
