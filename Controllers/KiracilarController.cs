using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmlakYonetim.Data;
using EmlakYonetim.Models;

namespace EmlakYonetim.Controllers;

public class KiracilarController : Controller
{
    private readonly EmlakYonetimDbContext _context;

    public KiracilarController(EmlakYonetimDbContext context)
    {
        _context = context;
    }

    // GET: Kiracilar
    public async Task<IActionResult> Index()
    {
        return View(await _context.Kiracilar.ToListAsync());
    }

    // GET: Kiracilar/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var kiraci = await _context.Kiracilar
            .Include(k => k.Kullanicilar)
            .Include(k => k.Musteriler)
            .Include(k => k.Emlaklar)
            .FirstOrDefaultAsync(m => m.KiraciID == id);

        if (kiraci == null)
        {
            return NotFound();
        }

        return View(kiraci);
    }

    // GET: Kiracilar/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Kiracilar/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirmaAd,LisansTipi,Aktif")] Kiraci kiraci)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(kiraci);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Kiracı başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Hata: {ex.Message}";
            }
        }
        else
        {
            // ModelState hatalarını topla
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["ErrorMessage"] = "Lütfen formu kontrol edin: " + string.Join(", ", errors);
        }
        return View(kiraci);
    }

    // GET: Kiracilar/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var kiraci = await _context.Kiracilar.FindAsync(id);
        if (kiraci == null)
        {
            return NotFound();
        }
        return View(kiraci);
    }

    // POST: Kiracilar/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("KiraciID,FirmaAd,LisansTipi,Aktif")] Kiraci kiraci)
    {
        if (id != kiraci.KiraciID)
        {
            return NotFound();
        }

        // Mevcut kaydı veritabanından çek
        var existingKiraci = await _context.Kiracilar.FindAsync(id);
        if (existingKiraci == null)
        {
            return NotFound();
        }

        // Navigation property hatalarını temizle
        ModelState.Remove("Kullanicilar");
        ModelState.Remove("Musteriler");
        ModelState.Remove("Emlaklar");

        if (ModelState.IsValid)
        {
            try
            {
                // Mevcut kaydın değerlerini güncelle
                existingKiraci.FirmaAd = kiraci.FirmaAd;
                existingKiraci.LisansTipi = kiraci.LisansTipi;
                existingKiraci.Aktif = kiraci.Aktif;

                _context.Update(existingKiraci);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Kiracı bilgileri başarıyla güncellendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KiraciExists(kiraci.KiraciID))
                {
                    return NotFound();
                }
                else
                {
                    TempData["ErrorMessage"] = "Güncelleme sırasında bir hata oluştu. Lütfen tekrar deneyin.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Hata: {ex.Message}";
                return View(kiraci);
            }
        }
        else
        {
            // ModelState hatalarını Türkçeleştir
            var errorMessages = new List<string>();
            foreach (var key in ModelState.Keys)
            {
                if (key == "Kullanicilar" || key == "Musteriler" || key == "Emlaklar") continue;
                
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    var fieldName = key;
                    if (fieldName == "FirmaAd") fieldName = "Firma Adı";
                    else if (fieldName == "LisansTipi") fieldName = "Lisans Tipi";
                    
                    errorMessages.Add($"{fieldName}: {error.ErrorMessage}");
                }
            }
            if (errorMessages.Any())
            {
                TempData["ErrorMessage"] = "Lütfen formu kontrol edin: " + string.Join(", ", errorMessages);
            }
        }
        return View(kiraci);
    }

    // GET: Kiracilar/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var kiraci = await _context.Kiracilar
            .FirstOrDefaultAsync(m => m.KiraciID == id);
        if (kiraci == null)
        {
            return NotFound();
        }

        return View(kiraci);
    }

    // POST: Kiracilar/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var kiraci = await _context.Kiracilar.FindAsync(id);
            if (kiraci != null)
            {
                _context.Kiracilar.Remove(kiraci);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Kiracı başarıyla silindi!";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Silme işlemi sırasında hata oluştu: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool KiraciExists(int id)
    {
        return _context.Kiracilar.Any(e => e.KiraciID == id);
    }
}


