using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Models;

[Table("DatPhong")]
[Index("MaDp", Name = "DatPhong_maDP", IsUnique = true)]
public partial class DatPhong
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("maDP")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDp { get; set; } = null!;

    [Column("soNguoi")]
    public int SoNguoi { get; set; }

    [Column("ngayBatDau", TypeName = "datetime")]
    public DateTime NgayBatDau { get; set; }

    [Column("ngayKetThuc", TypeName = "datetime")]
    public DateTime NgayKetThuc { get; set; }

    [Column("maKH")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaKh { get; set; } = null!;

    [Column("maP")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaP { get; set; } = null!;

    public virtual ICollection<DatDichVu> DatDichVus { get; } = new List<DatDichVu>();

    public virtual HoaDonDatPhong? HoaDonDatPhong { get; set; }

    public virtual HoaDonDichVu? HoaDonDichVu { get; set; }

    public virtual KhachHang MaKhNavigation { get; set; } = null!;

    public virtual Phong MaPNavigation { get; set; } = null!;
}
