using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakYonetim.Models;

[Table("Roller")]
public class Rol
{
    [Key]
    public int RolID { get; set; }

    [Required]
    [MaxLength(50)]
    public string RolAd { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Aciklama { get; set; }

    // Navigation properties
    public virtual ICollection<Kullanici> Kullanicilar { get; set; } = new List<Kullanici>();
    public virtual ICollection<RolYetki> RolYetkileri { get; set; } = new List<RolYetki>();
}

