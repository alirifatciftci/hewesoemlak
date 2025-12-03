using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakYonetim.Models;

[Table("Kullanicilar")]
public class Kullanici
{
    [Key]
    public int KullaniciID { get; set; }

    [Required]
    public int KiraciID { get; set; }

    [Required]
    public int RolID { get; set; }

    [Required]
    [MaxLength(100)]
    public string AdSoyad { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(256)]
    public string SifreHash { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefon { get; set; }

    public bool Aktif { get; set; } = true;

    [Required]
    public DateTime KayitTarihi { get; set; } = DateTime.Now;

    // Navigation properties
    [ForeignKey("KiraciID")]
    public virtual Kiraci Kiraci { get; set; } = null!;

    [ForeignKey("RolID")]
    public virtual Rol Rol { get; set; } = null!;

    // Danışman olarak verdiği emlaklar
    public virtual ICollection<Emlak> Emlaklar { get; set; } = new List<Emlak>();

    // Verdiği randevular
    public virtual ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
}

