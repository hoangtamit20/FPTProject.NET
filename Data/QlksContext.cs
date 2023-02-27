using System;
using System.Collections.Generic;
using Hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Data;

public partial class QlksContext : DbContext
{
    public QlksContext()
    {
    }

    public QlksContext(DbContextOptions<QlksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DatDichVu> DatDichVus { get; set; }

    public virtual DbSet<DatPhong> DatPhongs { get; set; }

    public virtual DbSet<DichVu> DichVus { get; set; }

    public virtual DbSet<HoaDonDatPhong> HoaDonDatPhongs { get; set; }

    public virtual DbSet<HoaDonDichVu> HoaDonDichVus { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LoaiPhong> LoaiPhongs { get; set; }

    public virtual DbSet<LuongNhanVien> LuongNhanViens { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TacVuPhong> TacVuPhongs { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:strCon");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DatDichVu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DatDichV__3213E83F72809520");

            entity.Property(e => e.MaDdv).IsFixedLength();
            entity.Property(e => e.MaDp).IsFixedLength();
            entity.Property(e => e.MaDv).IsFixedLength();
            entity.Property(e => e.NgayDatDichVu).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SoLuong).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.MaDpNavigation).WithMany(p => p.DatDichVus)
                .HasPrincipalKey(p => p.MaDp)
                .HasForeignKey(d => d.MaDp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKDatDichVu607749");

            entity.HasOne(d => d.MaDvNavigation).WithMany(p => p.DatDichVus)
                .HasPrincipalKey(p => p.MaDv)
                .HasForeignKey(d => d.MaDv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKDatDichVu404793");
        });

        modelBuilder.Entity<DatPhong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DatPhong__3213E83F17E765BD");

            entity.Property(e => e.MaDp).IsFixedLength();
            entity.Property(e => e.MaKh).IsFixedLength();
            entity.Property(e => e.MaP).IsFixedLength();
            entity.Property(e => e.NgayBatDau).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DatPhongs)
                .HasPrincipalKey(p => p.MaKh)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKDatPhong46426");

            entity.HasOne(d => d.MaPNavigation).WithMany(p => p.DatPhongs)
                .HasPrincipalKey(p => p.MaP)
                .HasForeignKey(d => d.MaP)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKDatPhong908988");
        });

        modelBuilder.Entity<DichVu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DichVu__3213E83F35A81529");

            entity.Property(e => e.MaDv).IsFixedLength();
        });

        modelBuilder.Entity<HoaDonDatPhong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HoaDonDa__3213E83FD56D33CD");

            entity.Property(e => e.MaDp).IsFixedLength();
            entity.Property(e => e.MaHddp).IsFixedLength();
            entity.Property(e => e.MaNv).IsFixedLength();
            entity.Property(e => e.NgayHd).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MaDpNavigation).WithOne(p => p.HoaDonDatPhong)
                .HasPrincipalKey<DatPhong>(p => p.MaDp)
                .HasForeignKey<HoaDonDatPhong>(d => d.MaDp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKHoaDonDatP734856");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDonDatPhongs)
                .HasPrincipalKey(p => p.MaNv)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKHoaDonDatP269546");
        });

        modelBuilder.Entity<HoaDonDichVu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HoaDonDi__3213E83FA6DFC297");

            entity.Property(e => e.MaDp).IsFixedLength();
            entity.Property(e => e.MaHddv).IsFixedLength();
            entity.Property(e => e.MaNv).IsFixedLength();
            entity.Property(e => e.NgayHoaDon).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MaDpNavigation).WithOne(p => p.HoaDonDichVu)
                .HasPrincipalKey<DatPhong>(p => p.MaDp)
                .HasForeignKey<HoaDonDichVu>(d => d.MaDp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKHoaDonDich791226");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDonDichVus)
                .HasPrincipalKey(p => p.MaNv)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKHoaDonDich175961");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KhachHan__3213E83FFD68D58C");

            entity.Property(e => e.MaKh).IsFixedLength();
        });

        modelBuilder.Entity<LoaiPhong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoaiPhon__3213E83FF2EF63C3");

            entity.Property(e => e.DonGia).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<LuongNhanVien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LuongNha__3213E83FF1C95C4F");

            entity.Property(e => e.MaNv).IsFixedLength();

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.LuongNhanViens)
                .HasPrincipalKey(p => p.MaNv)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLuongNhanV939854");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhanVien__3213E83F92DCF229");

            entity.Property(e => e.MaNv).IsFixedLength();

            entity.HasOne(d => d.UserNameNavigation).WithMany(p => p.NhanViens)
                .HasPrincipalKey(p => p.UserName)
                .HasForeignKey(d => d.UserName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKNhanVien442527");
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Phong__3213E83F1BFA9030");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MaP).IsFixedLength();
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MaLpNavigation).WithMany(p => p.Phongs)
                .HasPrincipalKey(p => p.MaLp)
                .HasForeignKey(d => d.MaLp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKPhong696843");

            entity.HasOne(d => d.MaTvpNavigation).WithMany(p => p.Phongs)
                .HasPrincipalKey(p => p.MaTvp)
                .HasForeignKey(d => d.MaTvp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKPhong845765");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3213E83FDA052078");
        });

        modelBuilder.Entity<TacVuPhong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TacVuPho__3213E83FD9DB1B3A");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaiKhoan__3213E83F1701417F");

            entity.Property(e => e.IdRole).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.TaiKhoans)
                .HasPrincipalKey(p => p.IdRole)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKTaiKhoan365359");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
