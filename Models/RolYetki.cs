using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakYonetim.Models;

[Table("RolYetkileri")]
public class RolYetki
{
    [Key]
    [Column(Order = 0)]
    public int RolID { get; set; }

    [Key]
    [Column(Order = 1)]
    public int YetkiID { get; set; }

    // Navigation properties
    [ForeignKey("RolID")]
    public virtual Rol Rol { get; set; } = null!;

    [ForeignKey("YetkiID")]
    public virtual Yetki Yetki { get; set; } = null!;
}

