using Microsoft.EntityFrameworkCore;

namespace ShortURL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ShortUrl> ShortUrls { get; set; }
}
