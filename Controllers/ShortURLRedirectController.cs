using Microsoft.AspNetCore.Mvc;
using ShortURL.Data;

[ApiController]
public class ShortURLRedirectController : ControllerBase
{
    private readonly AppDbContext _context;

    public ShortURLRedirectController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{shortCode}")]
    public IActionResult RedirectShortUrl(string shortCode)
    {
        var shortUrl = _context.ShortUrls.FirstOrDefault(s => s.ShortenedUrl == shortCode);
        if (shortUrl == null)
        {
            return NotFound();
        }

        shortUrl.Clicks++;
        _context.SaveChanges();

        return Redirect(shortUrl.OriginalUrl);
    }
}
