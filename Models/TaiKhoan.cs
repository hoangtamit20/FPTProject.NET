using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Models;

[Table("TaiKhoan")]
[Index("UserName", Name = "TaiKhoan_userName", IsUnique = true)]
public partial class TaiKhoan
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("userName")]
    [StringLength(225)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [Column("userPassword")]
    [StringLength(225)]
    [Unicode(false)]
    public string UserPassword { get; set; } = null!;

    [Column("idRole")]
    public int IdRole { get; set; }

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("phoneNumber")]
    [StringLength(11)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [Column("emailConfirm")]
    public bool EmailConfirm { get; set; }

    [Column("twoFactorEnabled")]
    public bool TwoFactorEnabled { get; set; }

    [Column("timeLockOut", TypeName = "datetime")]
    public DateTime? TimeLockOut { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<NhanVien> NhanViens { get; } = new List<NhanVien>();
}
