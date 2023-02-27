using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Models;

[Table("LuongNhanVien")]
public partial class LuongNhanVien
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ngayLamViec", TypeName = "datetime")]
    public DateTime NgayLamViec { get; set; }

    [Column("ngayNhanLuong", TypeName = "datetime")]
    public DateTime NgayNhanLuong { get; set; }

    [Column("soNgayLam")]
    public int SoNgayLam { get; set; }

    [Column("luongCung", TypeName = "decimal(19, 0)")]
    public decimal LuongCung { get; set; }

    [Column("ghiChu")]
    [StringLength(200)]
    public string? GhiChu { get; set; }

    [Column("maNV")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaNv { get; set; } = null!;

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
