using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakYonetim.Models;

[Table("Musteriler")]
public class Musteri
{
    [Key]
    public int MusteriID { get; set; }

    [Required]
    public int KiraciID { get; set; }

    [Required]
    [MaxLength(100)]
    public string AdSoyad { get; set; } = string.Empty;

    [MaxLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MaxLength(20)]
    public string Telefon { get; set; } = string.Empty;

    public string? Talepler { get; set; }

    [Required]
    public DateTime KayitTarihi { get; set; } = DateTime.Now;

    // Navigation property
    [ForeignKey("KiraciID")]
    public virtual Kiraci Kiraci { get; set; } = null!;

    // Randevuları
    public virtual ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
}

