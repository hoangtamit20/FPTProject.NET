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
    public class DichVuController : Controller
    {
        private readonly QlksdbContext _context;

        public DichVuController(QlksdbContext context)
        {
            _context = context;
        }

        // GET: DichVu
        public async Task<IActionResult> Index()
        {
              return _context.DichVus != null ? 
                          View(await _context.DichVus.ToListAsync()) :
                          Problem("Entity set 'QlksdbContext.DichVus'  is null.");
        }

        // GET: DichVu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DichVus == null)
            {
                return NotFound();
            }

            var dichVuModel = await _context.DichVus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dichVuModel == null)
            {
                return NotFound();
            }

            return View(dichVuModel);
        }

        // GET: DichVu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DichVu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaDv,TenDv,DonGia,SoLuong,HinhAnh")] DichVuModel dichVuModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dichVuModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dichVuModel);
        }

        // GET: DichVu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DichVus == null)
            {
                return NotFound();
            }

            var dichVuModel = await _context.DichVus.FindAsync(id);
            if (dichVuModel == null)
            {
                return NotFound();
            }
            return View(dichVuModel);
        }

        // POST: DichVu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaDv,TenDv,DonGia,SoLuong,HinhAnh")] DichVuModel dichVuModel)
        {
            if (id != dichVuModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dichVuModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DichVuModelExists(dichVuModel.Id))
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
            return View(dichVuModel);
        }

        // GET: DichVu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DichVus == null)
            {
                return NotFound();
            }

            var dichVuModel = await _context.DichVus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dichVuModel == null)
            {
                return NotFound();
            }

            return View(dichVuModel);
        }

        // POST: DichVu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DichVus == null)
            {
                return Problem("Entity set 'QlksdbContext.DichVus'  is null.");
            }
            var dichVuModel = await _context.DichVus.FindAsync(id);
            if (dichVuModel != null)
            {
                _context.DichVus.Remove(dichVuModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DichVuModelExists(int id)
        {
          return (_context.DichVus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
