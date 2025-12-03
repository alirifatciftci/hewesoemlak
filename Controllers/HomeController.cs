using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmlakYonetim.Models;
using EmlakYonetim.Data;

namespace EmlakYonetim.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly EmlakYonetimDbContext _context;

    public HomeController(ILogger<HomeController> logger, EmlakYonetimDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Varsayılan değerler
        ViewBag.KiraciCount = 0;
        ViewBag.EmlakCount = 0;
        ViewBag.MusteriCount = 0;
        ViewBag.DbConnected = false;

        try
        {
            // Veritabanı bağlantısını test et
            var kiraciCount = await _context.Kiracilar.CountAsync();
            var emlakCount = await _context.Emlaklar.CountAsync();
            var musteriCount = await _context.Musteriler.CountAsync();

            ViewBag.KiraciCount = kiraciCount;
            ViewBag.EmlakCount = emlakCount;
            ViewBag.MusteriCount = musteriCount;
            ViewBag.DbConnected = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Veritabanı bağlantı hatası");
            ViewBag.DbConnected = false;
            ViewBag.ErrorMessage = ex.Message;
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
