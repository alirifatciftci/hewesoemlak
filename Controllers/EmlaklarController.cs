using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmlakYonetim.Data;
using EmlakYonetim.Models;

namespace EmlakYonetim.Controllers;

public class EmlaklarController : Controller
{
    private readonly EmlakYonetimDbContext _context;

    public EmlaklarController(EmlakYonetimDbContext context)
    {
        _context = context;
    }

    // GET: Emlaklar
    public async Task<IActionResult> Index()
    {
        var emlaklar = await _context.Emlaklar
            .Include(e => e.Kiraci)
            .Include(e => e.Danisman)
            .Include(e => e.Durum)
            .Include(e => e.Tip)
            .ToListAsync();
        return View(emlaklar);
    }

    // GET: Emlaklar/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var emlak = await _context.Emlaklar
            .Include(e => e.Kiraci)
            .Include(e => e.Danisman)
            .Include(e => e.Durum)
            .Include(e => e.Tip)
            .Include(e => e.Randevular)
                .ThenInclude(r => r.Musteri)
            .FirstOrDefaultAsync(m => m.EmlakID == id);

        if (emlak == null)
        {
            return NotFound();
        }

        return View(emlak);
    }

    // GET: Emlaklar/Create
    public async Task<IActionResult> Create()
    {
        ViewData["KiraciID"] = new SelectList(await _context.Kiracilar.ToListAsync(), "KiraciID", "FirmaAd");
        ViewData["DanismanID"] = new SelectList(await _context.Kullanicilar.ToListAsync(), "KullaniciID", "AdSoyad");
        ViewData["DurumID"] = new SelectList(await _context.EmlakDurumlari.ToListAsync(), "DurumID", "DurumAd");
        ViewData["TipID"] = new SelectList(await _context.EmlakTipleri.ToListAsync(), "TipID", "TipAd");
        return View();
    }

    // POST: Emlaklar/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("KiraciID,DanismanID,DurumID,TipID,Baslik,Aciklama,Adres,Sehir,Fiyat,AlanMetrekare,OdaSayisi,Aktif")] Emlak emlak)
    {
        // Zorunlu alan kontrolleri
        if (emlak.KiraciID == 0)
        {
            ModelState.AddModelError("KiraciID", "Kiracı seçimi zorunludur.");
        }
        if (emlak.DanismanID == 0)
        {
            ModelState.AddModelError("DanismanID", "Danışman seçimi zorunludur.");
        }
        if (emlak.DurumID == 0)
        {
            ModelState.AddModelError("DurumID", "Durum seçimi zorunludur.");
        }
        if (emlak.TipID == 0)
        {
            ModelState.AddModelError("TipID", "Tip seçimi zorunludur.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                emlak.YayinTarihi = DateTime.Now;
                _context.Add(emlak);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Emlak başarıyla eklendi!";
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
                    else if (fieldName == "DanismanID") fieldName = "Danışman";
                    else if (fieldName == "DurumID") fieldName = "Durum";
                    else if (fieldName == "TipID") fieldName = "Tip";
                    else if (fieldName == "Baslik") fieldName = "Başlık";
                    else if (fieldName == "Adres") fieldName = "Adres";
                    else if (fieldName == "Sehir") fieldName = "Şehir";
                    else if (fieldName == "Fiyat") fieldName = "Fiyat";
                    
                    errorMessages.Add($"{fieldName}: {error.ErrorMessage}");
                }
            }
            TempData["ErrorMessage"] = "Lütfen formu kontrol edin: " + string.Join(", ", errorMessages);
        }
        ViewData["KiraciID"] = new SelectList(await _context.Kiracilar.ToListAsync(), "KiraciID", "FirmaAd", emlak.KiraciID);
        ViewData["DanismanID"] = new SelectList(await _context.Kullanicilar.ToListAsync(), "KullaniciID", "AdSoyad", emlak.DanismanID);
        ViewData["DurumID"] = new SelectList(await _context.EmlakDurumlari.ToListAsync(), "DurumID", "DurumAd", emlak.DurumID);
        ViewData["TipID"] = new SelectList(await _context.EmlakTipleri.ToListAsync(), "TipID", "TipAd", emlak.TipID);
        return View(emlak);
    }

    // GET: Emlaklar/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var emlak = await _context.Emlaklar.FindAsync(id);
        if (emlak == null)
        {
            return NotFound();
        }
        ViewData["KiraciID"] = new SelectList(await _context.Kiracilar.ToListAsync(), "KiraciID", "FirmaAd", emlak.KiraciID);
        ViewData["DanismanID"] = new SelectList(await _context.Kullanicilar.ToListAsync(), "KullaniciID", "AdSoyad", emlak.DanismanID);
        ViewData["DurumID"] = new SelectList(await _context.EmlakDurumlari.ToListAsync(), "DurumID", "DurumAd", emlak.DurumID);
        ViewData["TipID"] = new SelectList(await _context.EmlakTipleri.ToListAsync(), "TipID", "TipAd", emlak.TipID);
        return View(emlak);
    }

    // POST: Emlaklar/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("EmlakID,KiraciID,DanismanID,DurumID,TipID,Baslik,Aciklama,Adres,Sehir,Fiyat,AlanMetrekare,OdaSayisi,YayinTarihi,Aktif")] Emlak emlak)
    {
        if (id != emlak.EmlakID)
        {
            return NotFound();
        }

        // Mevcut kaydı veritabanından çek
        var existingEmlak = await _context.Emlaklar.FindAsync(id);
        if (existingEmlak == null)
        {
            return NotFound();
        }

        // Dropdown değerleri 0 ise mevcut değerleri kullan
        if (emlak.KiraciID == 0) emlak.KiraciID = existingEmlak.KiraciID;
        if (emlak.DanismanID == 0) emlak.DanismanID = existingEmlak.DanismanID;
        if (emlak.DurumID == 0) emlak.DurumID = existingEmlak.DurumID;
        if (emlak.TipID == 0) emlak.TipID = existingEmlak.TipID;

        // Navigation property hatalarını temizle
        ModelState.Remove("Kiraci");
        ModelState.Remove("Danisman");
        ModelState.Remove("Durum");
        ModelState.Remove("Tip");

        if (ModelState.IsValid)
        {
            try
            {
                // Mevcut kaydın değerlerini güncelle
                existingEmlak.KiraciID = emlak.KiraciID;
                existingEmlak.DanismanID = emlak.DanismanID;
                existingEmlak.DurumID = emlak.DurumID;
                existingEmlak.TipID = emlak.TipID;
                existingEmlak.Baslik = emlak.Baslik;
                existingEmlak.Aciklama = emlak.Aciklama;
                existingEmlak.Adres = emlak.Adres;
                existingEmlak.Sehir = emlak.Sehir;
                existingEmlak.Fiyat = emlak.Fiyat;
                existingEmlak.AlanMetrekare = emlak.AlanMetrekare;
                existingEmlak.OdaSayisi = emlak.OdaSayisi;
                existingEmlak.YayinTarihi = emlak.YayinTarihi;
                existingEmlak.Aktif = emlak.Aktif;

                _context.Update(existingEmlak);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Emlak bilgileri başarıyla güncellendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmlakExists(emlak.EmlakID))
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
                ViewData["KiraciID"] = new SelectList(await _context.Kiracilar.ToListAsync(), "KiraciID", "FirmaAd", emlak.KiraciID);
                ViewData["DanismanID"] = new SelectList(await _context.Kullanicilar.ToListAsync(), "KullaniciID", "AdSoyad", emlak.DanismanID);
                ViewData["DurumID"] = new SelectList(await _context.EmlakDurumlari.ToListAsync(), "DurumID", "DurumAd", emlak.DurumID);
                ViewData["TipID"] = new SelectList(await _context.EmlakTipleri.ToListAsync(), "TipID", "TipAd", emlak.TipID);
                return View(emlak);
            }
        }
        else
        {
            // ModelState hatalarını Türkçeleştir
            var errorMessages = new List<string>();
            foreach (var key in ModelState.Keys)
            {
                if (key == "Kiraci" || key == "Danisman" || key == "Durum" || key == "Tip") continue;
                
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    var fieldName = key;
                    if (fieldName == "KiraciID") fieldName = "Kiracı";
                    else if (fieldName == "DanismanID") fieldName = "Danışman";
                    else if (fieldName == "DurumID") fieldName = "Durum";
                    else if (fieldName == "TipID") fieldName = "Tip";
                    else if (fieldName == "Baslik") fieldName = "Başlık";
                    else if (fieldName == "Adres") fieldName = "Adres";
                    else if (fieldName == "Sehir") fieldName = "Şehir";
                    else if (fieldName == "Fiyat") fieldName = "Fiyat";
                    
                    errorMessages.Add($"{fieldName}: {error.ErrorMessage}");
                }
            }
            if (errorMessages.Any())
            {
                TempData["ErrorMessage"] = "Lütfen formu kontrol edin: " + string.Join(", ", errorMessages);
            }
        }
        ViewData["KiraciID"] = new SelectList(await _context.Kiracilar.ToListAsync(), "KiraciID", "FirmaAd", emlak.KiraciID);
        ViewData["DanismanID"] = new SelectList(await _context.Kullanicilar.ToListAsync(), "KullaniciID", "AdSoyad", emlak.DanismanID);
        ViewData["DurumID"] = new SelectList(await _context.EmlakDurumlari.ToListAsync(), "DurumID", "DurumAd", emlak.DurumID);
        ViewData["TipID"] = new SelectList(await _context.EmlakTipleri.ToListAsync(), "TipID", "TipAd", emlak.TipID);
        return View(emlak);
    }

    // GET: Emlaklar/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var emlak = await _context.Emlaklar
            .Include(e => e.Kiraci)
            .Include(e => e.Danisman)
            .Include(e => e.Durum)
            .Include(e => e.Tip)
            .FirstOrDefaultAsync(m => m.EmlakID == id);
        if (emlak == null)
        {
            return NotFound();
        }

        return View(emlak);
    }

    // POST: Emlaklar/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var emlak = await _context.Emlaklar.FindAsync(id);
            if (emlak != null)
            {
                _context.Emlaklar.Remove(emlak);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Emlak başarıyla silindi!";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Silme işlemi sırasında hata oluştu: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool EmlakExists(int id)
    {
        return _context.Emlaklar.Any(e => e.EmlakID == id);
    }
}


