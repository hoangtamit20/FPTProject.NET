using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Models;

[Table("HoaDonDatPhong")]
[Index("MaHddp", Name = "HoaDonDatPhong_maHDDP", IsUnique = true)]
[Index("MaDp", Name = "UQ__HoaDonDa__7A3EF41677BD53F1", IsUnique = true)]
public partial class HoaDonDatPhong
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("maHDDP")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaHddp { get; set; } = null!;

    [Column("ngayHD", TypeName = "datetime")]
    public DateTime NgayHd { get; set; }

    [Column("maNV")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaNv { get; set; } = null!;

    [Column("maDP")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDp { get; set; } = null!;

    public virtual DatPhong MaDpNavigation { get; set; } = null!;

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
