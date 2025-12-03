using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmlakYonetim.Data;
using EmlakYonetim.Models;

namespace EmlakYonetim.Controllers;

public class MusterilerController : Controller
{
    private readonly EmlakYonetimDbContext _context;

    public MusterilerController(EmlakYonetimDbContext context)
    {
        _context = context;
    }

    // GET: Musteriler
    public async Task<IActionResult> Index()
    {
        var musteriler = await _context.Musteriler
            .Include(m => m.Kiraci)
            .ToListAsync();
        return View(musteriler);
    }

    // GET: Musteriler/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var musteri = await _context.Musteriler
            .Include(m => m.Kiraci)
            .Include(m => m.Randevular)
                .ThenInclude(r => r.Danisman)
            .Include(m => m.Randevular)
                .ThenInclude(r => r.Emlak)
            .FirstOrDefaultAsync(m => m.MusteriID == id);

        if (musteri == null)
        {
            return NotFound();
        }

        return View(musteri);
    }

    // GET: Musteriler/Create
    public async Task<IActionResult> Create()
    {
        ViewData["KiraciID"] = new SelectList(await _context.Kiracilar.ToListAsync(), "KiraciID", "FirmaAd");
        return View();
    }

    // POST: Musteriler/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("KiraciID,AdSoyad,Email,Telefon,Talepler")] Musteri musteri)
    {
        // KiraciID kontrolü
        if (musteri.KiraciID == 0)
        {
            ModelState.AddModelError("KiraciID", "Kiracı seçimi zorunludur.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                musteri.KayitTarihi = DateTime.Now;
                _context.Add(musteri);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Müşteri başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Hata: {ex.Message}";
            }
        }
        else
        {
            // ModelState hatalarını Türkçeleştir
            var errorMessages = new List<string>();
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    var fieldName = key;
                    if (fieldName == "KiraciID") fieldName = "Kiracı";
                    else if (fieldName == "AdSoyad") fieldName = "Ad Soyad";
                    else if (fieldName == "Telefon") fieldName = "Telefon";
                    
                    errorMessages.Add($"{fieldName}: {error.ErrorMessage}");
                }
            }
            TempData["ErrorMessage"] = "Lütfen formu kontrol edin: " + string.Join(", ", errorMessages);
        }
        ViewData["KiraciID"] = new SelectList(await _context.Kiracilar.ToListAsync(), "KiraciID", "FirmaAd", musteri.KiraciID);
        return View(musteri);
    }

    // GET: Musteriler/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var musteri = await _context.Musteriler.FindAsync(id);
        if (musteri == null)
        {
            return NotFound();
        }
        ViewData["KiraciID"] = new SelectList(await _context.Kiracilar.ToListAsync(), "KiraciID", "FirmaAd", musteri.KiraciID);
        return View(musteri);
    }

    // POST: Musteriler/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("MusteriID,KiraciID,AdSoyad,Email,Telefon,Talepler,KayitTarihi")] Musteri musteri)
    {
        if (id != musteri.MusteriID)
        {
            return NotFound();
        }

        // Mevcut kaydı veritabanından çek
        var existingMusteri = await _context.Musteriler.FindAsync(id);
        if (existingMusteri == null)
        {
            return NotFound();
        }

        // KiraciID kontrolü - eğer 0 ise mevcut değeri kullan
        if (musteri.KiraciID == 0)
        {
            musteri.KiraciID = existingMusteri.KiraciID;
        }

        // ModelState'i temizle ve sadece gerçek hataları kontrol et
        ModelState.Remove("Kiraci");
        
        if (ModelState.IsValid)
        {
            try
            {
                // Mevcut kaydın değerlerini güncelle
                existingMusteri.KiraciID = musteri.KiraciID;
                existingMusteri.AdSoyad = musteri.AdSoyad;
                existingMusteri.Email = musteri.Email;
                existingMusteri.Telefon = musteri.Telefon;
                existingMusteri.Talepler = musteri.Talepler;
                existingMusteri.KayitTarihi = musteri.KayitTarihi;

                _context.Update(existingMusteri);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Müşteri bilgileri başarıyla güncellendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusteriExists(musteri.MusteriID))
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
                ViewData["KiraciID"] = new SelectList(await _context.Kiracilar.ToListAsync(), "KiraciID", "FirmaAd", musteri.KiraciID);
                return View(musteri);
            }
        }
        else
        {
            // ModelState hatalarını Türkçeleştir
            var errorMessages = new List<string>();
            foreach (var key in ModelState.Keys)
            {
                if (key == "Kiraci") continue; // Navigation property hatasını atla
                
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    var fieldName = key;
                    if (fieldName == "KiraciID") fieldName = "Kiracı";
                    else if (fieldName == "AdSoyad") fieldName = "Ad Soyad";
                    else if (fieldName == "Telefon") fieldName = "Telefon";
                    
                    errorMessages.Add($"{fieldName}: {error.ErrorMessage}");
                }
            }
            if (errorMessages.Any())
            {
                TempData["ErrorMessage"] = "Lütfen formu kontrol edin: " + string.Join(", ", errorMessages);
            }
        }
        ViewData["KiraciID"] = new SelectList(await _context.Kiracilar.ToListAsync(), "KiraciID", "FirmaAd", musteri.KiraciID);
        return View(musteri);
    }

    // GET: Musteriler/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var musteri = await _context.Musteriler
            .Include(m => m.Kiraci)
            .FirstOrDefaultAsync(m => m.MusteriID == id);
        if (musteri == null)
        {
            return NotFound();
        }

        return View(musteri);
    }

    // POST: Musteriler/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var musteri = await _context.Musteriler.FindAsync(id);
            if (musteri != null)
            {
                _context.Musteriler.Remove(musteri);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Müşteri başarıyla silindi!";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Silme işlemi sırasında hata oluştu: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool MusteriExists(int id)
    {
        return _context.Musteriler.Any(e => e.MusteriID == id);
    }
}


