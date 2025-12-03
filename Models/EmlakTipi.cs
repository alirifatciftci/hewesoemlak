using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakYonetim.Models;

[Table("EmlakTipleri")]
public class EmlakTipi
{
    [Key]
    public int TipID { get; set; }

    [Required]
    [MaxLength(50)]
    public string TipAd { get; set; } = string.Empty;

    // Navigation property
    public virtual ICollection<Emlak> Emlaklar { get; set; } = new List<Emlak>();
}

