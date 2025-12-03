using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakYonetim.Models;

[Table("Yetkiler")]
public class Yetki
{
    [Key]
    public int YetkiID { get; set; }

    [Required]
    [MaxLength(100)]
    public string YetkiKod { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Aciklama { get; set; }

    // Navigation property
    public virtual ICollection<RolYetki> RolYetkileri { get; set; } = new List<RolYetki>();
}

