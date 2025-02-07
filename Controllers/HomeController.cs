using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShortURL.Data;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var urls = await _context.ShortUrls.ToListAsync();
        return View(urls);
    }
}
