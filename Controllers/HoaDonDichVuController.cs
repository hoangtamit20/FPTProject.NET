using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class HoaDonDichVuController : Controller
    {
        private readonly QlksdbContext _context;

        public HoaDonDichVuController(QlksdbContext context)
        {
            _context = context;
        }

        // GET: HoaDonDichVu
        public async Task<IActionResult> Index()
        {
            if (_context.HoaDonDichVus != null)
            {
                var qlksdbContext = _context.HoaDonDichVus.Include(h => h.MaDpNavigation).Include(h => h.MaNvNavigation);
                return View(await qlksdbContext.ToListAsync());
            }
            return View();
        }

        // GET: HoaDonDichVu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HoaDonDichVus == null)
            {
                return NotFound();
            }

            var hoaDonDichVuModel = await _context.HoaDonDichVus
                .Include(h => h.MaDpNavigation)
                .Include(h => h.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hoaDonDichVuModel == null)
            {
                return NotFound();
            }

            return View(hoaDonDichVuModel);
        }

        // GET: HoaDonDichVu/Create
        public IActionResult Create()
        {
            ViewData["MaDp"] = new SelectList(_context.DatPhongs, "MaDp", "MaDp");
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv");
            return View();
        }

        // POST: HoaDonDichVu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaHddv,NgayHoaDon,MaNv,MaDp")] HoaDonDichVuModel hoaDonDichVuModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDonDichVuModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDp"] = new SelectList(_context.DatPhongs, "MaDp", "MaDp", hoaDonDichVuModel.MaDp);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDonDichVuModel.MaNv);
            return View(hoaDonDichVuModel);
        }

        // GET: HoaDonDichVu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HoaDonDichVus == null)
            {
                return NotFound();
            }

            var hoaDonDichVuModel = await _context.HoaDonDichVus.FindAsync(id);
            if (hoaDonDichVuModel == null)
            {
                return NotFound();
            }
            ViewData["MaDp"] = new SelectList(_context.DatPhongs, "MaDp", "MaDp", hoaDonDichVuModel.MaDp);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDonDichVuModel.MaNv);
            return View(hoaDonDichVuModel);
        }

        // POST: HoaDonDichVu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaHddv,NgayHoaDon,MaNv,MaDp")] HoaDonDichVuModel hoaDonDichVuModel)
        {
            if (id != hoaDonDichVuModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDonDichVuModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonDichVuModelExists(hoaDonDichVuModel.Id))
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
            ViewData["MaDp"] = new SelectList(_context.DatPhongs, "MaDp", "MaDp", hoaDonDichVuModel.MaDp);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDonDichVuModel.MaNv);
            return View(hoaDonDichVuModel);
        }

        // GET: HoaDonDichVu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HoaDonDichVus == null)
            {
                return NotFound();
            }

            var hoaDonDichVuModel = await _context.HoaDonDichVus
                .Include(h => h.MaDpNavigation)
                .Include(h => h.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hoaDonDichVuModel == null)
            {
                return NotFound();
            }

            return View(hoaDonDichVuModel);
        }

        // POST: HoaDonDichVu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HoaDonDichVus == null)
            {
                return Problem("Entity set 'QlksdbContext.HoaDonDichVus'  is null.");
            }
            var hoaDonDichVuModel = await _context.HoaDonDichVus.FindAsync(id);
            if (hoaDonDichVuModel != null)
            {
                _context.HoaDonDichVus.Remove(hoaDonDichVuModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonDichVuModelExists(int id)
        {
            return (_context.HoaDonDichVus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
