using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakYonetim.Models;

[Table("Randevular")]
public class Randevu
{
    [Key]
    public int RandevuID { get; set; }

    public int? EmlakID { get; set; }

    [Required]
    public int MusteriID { get; set; }

    [Required]
    public int DanismanID { get; set; }

    [Required]
    public DateTime BaslangicZamani { get; set; }

    [Required]
    public DateTime BitisZamani { get; set; }

    [Required]
    [MaxLength(200)]
    public string Konu { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Durum { get; set; } = "Planlandı"; // Planlandı, Tamamlandı, İptal Edildi

    // Navigation properties
    [ForeignKey("EmlakID")]
    public virtual Emlak? Emlak { get; set; }

    [ForeignKey("MusteriID")]
    public virtual Musteri Musteri { get; set; } = null!;

    [ForeignKey("DanismanID")]
    public virtual Kullanici Danisman { get; set; } = null!;
}

