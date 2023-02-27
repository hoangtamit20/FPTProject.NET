using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Models;

[Table("DatDichVu")]
[Index("MaDdv", Name = "DatDichVu_maDDV", IsUnique = true)]
public partial class DatDichVu
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("maDDV")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDdv { get; set; } = null!;

    [Column("soLuong")]
    public int SoLuong { get; set; }

    [Column("ngayDatDichVu", TypeName = "datetime")]
    public DateTime NgayDatDichVu { get; set; }

    [Column("maDV")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDv { get; set; } = null!;

    [Column("maDP")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDp { get; set; } = null!;

    public virtual DatPhong MaDpNavigation { get; set; } = null!;

    public virtual DichVu MaDvNavigation { get; set; } = null!;
}
