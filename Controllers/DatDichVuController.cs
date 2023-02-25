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

        // GET: DatDichVu
        public async Task<IActionResult> Index()
        {
            var qlksdbContext = _context.DatDichVus.Include(d => d.MaDpNavigation).Include(d => d.MaDvNavigation);
            return View(await qlksdbContext.ToListAsync());
        }

        // GET: DatDichVu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DatDichVus == null)
            {
                return NotFound();
            }

            var datDichVuModel = await _context.DatDichVus
                .Include(d => d.MaDpNavigation)
                .Include(d => d.MaDvNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datDichVuModel == null)
            {
                return NotFound();
            }

            return View(datDichVuModel);
        }

        // GET: DatDichVu/Create
        public IActionResult Create()
        {
            ViewData["MaDp"] = new SelectList(_context.DatPhongs, "MaDp", "MaDp");
            ViewData["MaDv"] = new SelectList(_context.DichVus, "MaDv", "MaDv");
            return View();
        }

        // POST: DatDichVu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaDdv,SoLuong,NgayDatDichVu,MaDv,MaDp")] DatDichVuModel datDichVuModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datDichVuModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDp"] = new SelectList(_context.DatPhongs, "MaDp", "MaDp", datDichVuModel.MaDp);
            ViewData["MaDv"] = new SelectList(_context.DichVus, "MaDv", "MaDv", datDichVuModel.MaDv);
            return View(datDichVuModel);
        }

        // GET: DatDichVu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DatDichVus == null)
            {
                return NotFound();
            }

            var datDichVuModel = await _context.DatDichVus.FindAsync(id);
            if (datDichVuModel == null)
            {
                return NotFound();
            }
            ViewData["MaDp"] = new SelectList(_context.DatPhongs, "MaDp", "MaDp", datDichVuModel.MaDp);
            ViewData["MaDv"] = new SelectList(_context.DichVus, "MaDv", "MaDv", datDichVuModel.MaDv);
            return View(datDichVuModel);
        }

        // POST: DatDichVu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaDdv,SoLuong,NgayDatDichVu,MaDv,MaDp")] DatDichVuModel datDichVuModel)
        {
            if (id != datDichVuModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datDichVuModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatDichVuModelExists(datDichVuModel.Id))
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
            ViewData["MaDp"] = new SelectList(_context.DatPhongs, "MaDp", "MaDp", datDichVuModel.MaDp);
            ViewData["MaDv"] = new SelectList(_context.DichVus, "MaDv", "MaDv", datDichVuModel.MaDv);
            return View(datDichVuModel);
        }

        // GET: DatDichVu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DatDichVus == null)
            {
                return NotFound();
            }

            var datDichVuModel = await _context.DatDichVus
                .Include(d => d.MaDpNavigation)
                .Include(d => d.MaDvNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datDichVuModel == null)
            {
                return NotFound();
            }

            return View(datDichVuModel);
        }

        // POST: DatDichVu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DatDichVus == null)
            {
                return Problem("Entity set 'QlksdbContext.DatDichVus'  is null.");
            }
            var datDichVuModel = await _context.DatDichVus.FindAsync(id);
            if (datDichVuModel != null)
            {
                _context.DatDichVus.Remove(datDichVuModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatDichVuModelExists(int id)
        {
          return (_context.DatDichVus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
