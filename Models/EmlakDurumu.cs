using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakYonetim.Models;

[Table("EmlakDurumlari")]
public class EmlakDurumu
{
    [Key]
    public int DurumID { get; set; }

    [Required]
    [MaxLength(50)]
    public string DurumAd { get; set; } = string.Empty;

    // Navigation property
    public virtual ICollection<Emlak> Emlaklar { get; set; } = new List<Emlak>();
}

