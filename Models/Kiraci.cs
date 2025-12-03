using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakYonetim.Models;

[Table("Kiracilar")]
public class Kiraci
{
    [Key]
    public int KiraciID { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirmaAd { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LisansTipi { get; set; } = string.Empty;

    public bool Aktif { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Kullanici> Kullanicilar { get; set; } = new List<Kullanici>();
    public virtual ICollection<Musteri> Musteriler { get; set; } = new List<Musteri>();
    public virtual ICollection<Emlak> Emlaklar { get; set; } = new List<Emlak>();
}

