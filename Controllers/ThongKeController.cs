using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hotel.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hotel.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly ILogger<ThongKeController> _logger;
        private readonly QlksdbContext _context;
        public ThongKeController(ILogger<ThongKeController> logger, QlksdbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var tongSoLuongKhachHang = _context.KhachHangs.Count();
            var tongDoanhThuTheoThang = _context.HoaDonDatPhongs.Include(hd => hd.MaDpNavigation).
                                        Include(hd => hd.MaDpNavigation.MaPNavigation).
                                        Include(hd => hd.MaDpNavigation.MaPNavigation.MaLpNavigation).
                                        Where(hd => hd.NgayHd.Month == 2 && hd.NgayHd.Year == DateTime.Now.Year).
                                        Sum(hd => (hd.NgayHd.Day - hd.MaDpNavigation.NgayBatDau.Day)*hd.MaDpNavigation.MaPNavigation.MaLpNavigation.DonGia);
            var tongTienDichVu = _context.DatDichVus.Include(ddv => ddv.MaDvNavigation).Where(ddv => ddv.NgayDatDichVu.Month == 2).Sum(ddv => ddv.SoLuong * ddv.MaDvNavigation.DonGia);
            var kq = _context.DatPhongs.
                                        Where(dp => dp.NgayBatDau.Month == 2 && dp.NgayBatDau.Year == DateTime.Now.Year).
                                        GroupBy(dp => dp.MaKh).Count();
            
            object[] kq1 = {tongSoLuongKhachHang, kq, tongDoanhThuTheoThang, tongTienDichVu};

            return View(kq1);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}