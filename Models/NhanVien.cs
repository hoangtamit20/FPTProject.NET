using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Models;

[Table("NhanVien")]
[Index("MaNv", Name = "NhanVien_maNV", IsUnique = true)]
[Index("CCccd", Name = "UQ__NhanVien__8BCCF4CBAB68F0D9", IsUnique = true)]
public partial class NhanVien
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("maNV")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaNv { get; set; } = null!;

    [Column("tenNV")]
    [StringLength(50)]
    public string TenNv { get; set; } = null!;

    [Column("hinhAnh", TypeName = "ntext")]
    public string? HinhAnh { get; set; }

    [Column("soDT")]
    [StringLength(11)]
    [Unicode(false)]
    public string SoDt { get; set; } = null!;

    [Column("cCCCD")]
    [StringLength(12)]
    [Unicode(false)]
    public string CCccd { get; set; } = null!;

    [Column("diaChi")]
    [StringLength(255)]
    public string? DiaChi { get; set; }

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("userName")]
    [StringLength(225)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    public virtual ICollection<HoaDonDatPhong> HoaDonDatPhongs { get; } = new List<HoaDonDatPhong>();

    public virtual ICollection<HoaDonDichVu> HoaDonDichVus { get; } = new List<HoaDonDichVu>();

    public virtual ICollection<LuongNhanVien> LuongNhanViens { get; } = new List<LuongNhanVien>();

    public virtual TaiKhoan UserNameNavigation { get; set; } = null!;
}
