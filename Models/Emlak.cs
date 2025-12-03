using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakYonetim.Models;

[Table("Emlaklar")]
public class Emlak
{
    [Key]
    public int EmlakID { get; set; }

    [Required]
    public int KiraciID { get; set; }

    [Required]
    public int DanismanID { get; set; }

    [Required]
    public int DurumID { get; set; }

    [Required]
    public int TipID { get; set; }

    [Required]
    [MaxLength(200)]
    public string Baslik { get; set; } = string.Empty;

    public string? Aciklama { get; set; }

    [Required]
    [MaxLength(255)]
    public string Adres { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Sehir { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Fiyat { get; set; }

    public int? AlanMetrekare { get; set; }

    public int? OdaSayisi { get; set; }

    [Required]
    public DateTime YayinTarihi { get; set; } = DateTime.Now;

    public bool Aktif { get; set; } = true;

    // Navigation properties
    [ForeignKey("KiraciID")]
    public virtual Kiraci Kiraci { get; set; } = null!;

    [ForeignKey("DanismanID")]
    public virtual Kullanici Danisman { get; set; } = null!;

    [ForeignKey("DurumID")]
    public virtual EmlakDurumu Durum { get; set; } = null!;

    [ForeignKey("TipID")]
    public virtual EmlakTipi Tip { get; set; } = null!;

    // Randevuları
    public virtual ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
}

