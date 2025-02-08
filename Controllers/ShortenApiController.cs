using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using ShortURL.Data;

[ApiController]
[Route("api/[controller]")]
public class ShortenApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public ShortenApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.ShortUrls.ToList());
    }

    [HttpPost]
    public IActionResult Create([FromBody] ShortUrlDto request)
    {
        if (string.IsNullOrEmpty(request.OriginalUrl) || !Uri.IsWellFormedUriString(request.OriginalUrl, UriKind.Absolute))
        {
            return BadRequest("Invalid URL");
        }

        var shortCode = GenerateShortCode();
        var shortUrl = new ShortUrl { OriginalUrl = request.OriginalUrl, ShortenedUrl = shortCode };

        _context.ShortUrls.Add(shortUrl);
        _context.SaveChanges();

        return Ok(shortUrl);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var url = _context.ShortUrls.Find(id);
        if (url == null)
        {
            return NotFound();
        }
        _context.ShortUrls.Remove(url);
        _context.SaveChanges();
        return NoContent();
    }

    private string GenerateShortCode()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[6];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes).Replace("/", "_").Replace("+", "-").Substring(0, 6);
    }

    public class ShortUrlDto
    {
        public required string OriginalUrl { get; set; }
    }
}
