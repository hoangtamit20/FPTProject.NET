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
    public class PhongController : Controller
    {
        private readonly QlksdbContext _context;

        public PhongController(QlksdbContext context)
        {
            _context = context;
        }

        // GET: Phong
        public async Task<IActionResult> Index()
        {
            var qlksdbContext = _context.Phongs.Include(p => p.MaLpNavigation).Include(p => p.MaTvpNavigation);
            return View(await qlksdbContext.ToListAsync());
        }

        // GET: Phong/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Phongs == null)
            {
                return NotFound();
            }

            var phongModel = await _context.Phongs
                .Include(p => p.MaLpNavigation)
                .Include(p => p.MaTvpNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phongModel == null)
            {
                return NotFound();
            }

            return View(phongModel);
        }

        // GET: Phong/Create
        public IActionResult Create()
        {
            ViewData["MaLp"] = new SelectList(_context.LoaiPhongs, "MaLp", "MaLp");
            ViewData["MaTvp"] = new SelectList(_context.TacVuPhongs, "MaTvp", "MaTvp");
            return View();
        }

        // POST: Phong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaP,SoPhong,HinhAnh,MaLp,MaTvp,TrangThaiPhong,MoTa,LoaiGiuong,CreatedAt,UpdatedAt")] PhongModel phongModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phongModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLp"] = new SelectList(_context.LoaiPhongs, "MaLp", "MaLp", phongModel.MaLp);
            ViewData["MaTvp"] = new SelectList(_context.TacVuPhongs, "MaTvp", "MaTvp", phongModel.MaTvp);
            return View(phongModel);
        }

        // GET: Phong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Phongs == null)
            {
                return NotFound();
            }

            var phongModel = await _context.Phongs.FindAsync(id);
            if (phongModel == null)
            {
                return NotFound();
            }
            ViewData["MaLp"] = new SelectList(_context.LoaiPhongs, "MaLp", "MaLp", phongModel.MaLp);
            ViewData["MaTvp"] = new SelectList(_context.TacVuPhongs, "MaTvp", "MaTvp", phongModel.MaTvp);
            return View(phongModel);
        }

        // POST: Phong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaP,SoPhong,HinhAnh,MaLp,MaTvp,TrangThaiPhong,MoTa,LoaiGiuong,CreatedAt,UpdatedAt")] PhongModel phongModel)
        {
            if (id != phongModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phongModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhongModelExists(phongModel.Id))
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
            ViewData["MaLp"] = new SelectList(_context.LoaiPhongs, "MaLp", "MaLp", phongModel.MaLp);
            ViewData["MaTvp"] = new SelectList(_context.TacVuPhongs, "MaTvp", "MaTvp", phongModel.MaTvp);
            return View(phongModel);
        }

        // GET: Phong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Phongs == null)
            {
                return NotFound();
            }

            var phongModel = await _context.Phongs
                .Include(p => p.MaLpNavigation)
                .Include(p => p.MaTvpNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phongModel == null)
            {
                return NotFound();
            }

            return View(phongModel);
        }

        // POST: Phong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Phongs == null)
            {
                return Problem("Entity set 'QlksdbContext.Phongs'  is null.");
            }
            var phongModel = await _context.Phongs.FindAsync(id);
            if (phongModel != null)
            {
                _context.Phongs.Remove(phongModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhongModelExists(int id)
        {
          return (_context.Phongs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
