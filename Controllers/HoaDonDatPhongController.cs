using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotel.Base;
using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class HoaDonDatPhongController : BaseController
    {
        public HoaDonDatPhongController(QlksdbContext context) : base(context)
        {
        }

        // GET: HoaDonDatPhong
        public async Task<IActionResult> Index()
        {
            if (base.KiemTraPhanQuyen("Admin") || base.KiemTraPhanQuyen("NhanVien") || base.KiemTraPhanQuyen("QuanLy"))
            {
                // System.Console.WriteLine("da vao day!");
                if (_context.HoaDonDatPhongs != null)
                {
                    var qlksdbContext = _context.HoaDonDatPhongs.Include(h => h.MaDpNavigation).Include(h => h.MaNvNavigation);
                    return View(await qlksdbContext.ToListAsync());
                }
                return View();
            }
            return base.ChuyenHuong();
        }

        // GET: HoaDonDatPhong/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (base.KiemTraPhanQuyen("Admin") || base.KiemTraPhanQuyen("NhanVien") || base.KiemTraPhanQuyen("QuanLy"))
            {
                if (id == null || _context.HoaDonDatPhongs == null)
                {
                    return NotFound();
                }

                var hoaDonDatPhongModel = await _context.HoaDonDatPhongs
                    .Include(h => h.MaDpNavigation)
                    .Include(h => h.MaNvNavigation)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (hoaDonDatPhongModel == null)
                {
                    return NotFound();
                }

                return View(hoaDonDatPhongModel);
            }
            return base.ChuyenHuong();
        }

        // GET: HoaDonDatPhong/Create
        public IActionResult Create()
        {
            if (base.KiemTraPhanQuyen("Admin") || base.KiemTraPhanQuyen("QuanLy"))
            {
                ViewData["MaDp"] = new SelectList(_context.DatPhongs, "MaDp", "MaDp");
                ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv");
                return View();
            }
            return ChuyenHuong();
        }

        // POST: HoaDonDatPhong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaHddp,NgayHd,MaNv,MaDp")] HoaDonDatPhongModel hoaDonDatPhongModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDonDatPhongModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDp"] = new SelectList(_context.DatPhongs, "MaDp", "MaDp", hoaDonDatPhongModel.MaDp);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDonDatPhongModel.MaNv);
            return View(hoaDonDatPhongModel);
        }

        // GET: HoaDonDatPhong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (base.KiemTraPhanQuyen("Admin") || base.KiemTraPhanQuyen("NhanVien") || base.KiemTraPhanQuyen("QuanLy"))
            {
                if (id == null || _context.HoaDonDatPhongs == null)
                {
                    return NotFound();
                }

                var hoaDonDatPhongModel = await _context.HoaDonDatPhongs.FindAsync(id);
                if (hoaDonDatPhongModel == null)
                {
                    return NotFound();
                }
                ViewData["MaDp"] = new SelectList(_context.DatPhongs, "MaDp", "MaDp", hoaDonDatPhongModel.MaDp);
                ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDonDatPhongModel.MaNv);
                return View(hoaDonDatPhongModel);
            }
            return base.ChuyenHuong();
        }

        // POST: HoaDonDatPhong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaHddp,NgayHd,MaNv,MaDp")] HoaDonDatPhongModel hoaDonDatPhongModel)
        {
            if (id != hoaDonDatPhongModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDonDatPhongModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonDatPhongModelExists(hoaDonDatPhongModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDp"] = new SelectList(_context.DatPhongs, "MaDp", "MaDp", hoaDonDatPhongModel.MaDp);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDonDatPhongModel.MaNv);
            return View(hoaDonDatPhongModel);
        }

        // GET: HoaDonDatPhong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (base.KiemTraPhanQuyen("Admin") || base.KiemTraPhanQuyen("QuanLy"))
            {
                if (id == null || _context.HoaDonDatPhongs == null)
                {
                    return NotFound();
                }

                var hoaDonDatPhongModel = await _context.HoaDonDatPhongs
                    .Include(h => h.MaDpNavigation)
                    .Include(h => h.MaNvNavigation)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (hoaDonDatPhongModel == null)
                {
                    return NotFound();
                }

                return View(hoaDonDatPhongModel);
            }
            return base.ChuyenHuong();
        }

        // POST: HoaDonDatPhong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (base.KiemTraPhanQuyen("Admin") || base.KiemTraPhanQuyen("QuanLy"))
            {
                if (_context.HoaDonDatPhongs == null)
                {
                    return Problem("Entity set 'QlksdbContext.HoaDonDatPhongs'  is null.");
                }
                var hoaDonDatPhongModel = await _context.HoaDonDatPhongs.FindAsync(id);
                if (hoaDonDatPhongModel != null)
                {
                    _context.HoaDonDatPhongs.Remove(hoaDonDatPhongModel);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return base.ChuyenHuong();
        }

        private bool HoaDonDatPhongModelExists(int id)
        {
            return (_context.HoaDonDatPhongs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
