using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Models;

namespace Hotel.Controllers
{
    public class DatDichVuController : Controller
    {
        private readonly QlksdbContext _context;

        public DatDichVuController(QlksdbContext context)
        {
            _context = context;
        }

        public void taoMaDdv()
        {
            var datDv = _context.DatDichVus.OrderByDescending(dv => dv.Id).FirstOrDefault();
            if (datDv != null)
            {
                ViewData["MaDdv"] = "DDV" + datDv.Id.ToString();
            }
            else
            {
                ViewData["MaDdv"] = "DDV1";
            }
        }

        public IActionResult Index()
        {
            System.Console.WriteLine("abc");
            if (_context.DatDichVus == null)
            {
                return Problem("Chưa tồn tại phòng nào");
            }
            var datDichVu = _context.DatDichVus.OrderByDescending(ddv => ddv.MaDdv).ToList();
            return View(datDichVu);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
        [HttpGet]
        public IActionResult Create(int? idDP)
        {
            var datP = _context.DatPhongs.FirstOrDefault(p => p.Id == idDP);
            var phong = _context.Phongs.FirstOrDefault(dp=>dp.MaP == datP.MaP);
            ViewData["MaDp"] = datP.MaDp;
            ViewData["SoPhong"] = phong.SoPhong;
            ViewData["MaDv"] = new SelectList(_context.DichVus, "MaDv", "TenDv");
            taoMaDdv();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DatDichVuModel datdichvu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datdichvu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
