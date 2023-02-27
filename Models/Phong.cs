using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Models;

[Table("Phong")]
[Index("MaP", Name = "Phong_maP", IsUnique = true)]
[Index("SoPhong", Name = "UQ__Phong__68583FAD855740B9", IsUnique = true)]
public partial class Phong
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("maP")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaP { get; set; } = null!;

    [Column("soPhong")]
    public int SoPhong { get; set; }

    [Column("hinhAnh", TypeName = "ntext")]
    public string? HinhAnh { get; set; }

    [Column("maLP")]
    public int MaLp { get; set; }

    [Column("maTVP")]
    public int MaTvp { get; set; }

    [Column("trangThaiPhong")]
    public int TrangThaiPhong { get; set; }

    [Column("moTa")]
    [StringLength(500)]
    public string? MoTa { get; set; }

    [Column("loaiGiuong")]
    public bool LoaiGiuong { get; set; }

    [Column("createdAt", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updatedAt", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<DatPhong> DatPhongs { get; } = new List<DatPhong>();

    public virtual LoaiPhong MaLpNavigation { get; set; } = null!;

    public virtual TacVuPhong MaTvpNavigation { get; set; } = null!;
}
