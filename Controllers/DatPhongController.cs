using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SelectPdf;

namespace Hotel.Controllers
{
    public class DatPhongController : Controller
    {
        private readonly QlksdbContext _context;

        public DatPhongController(QlksdbContext context)
        {
            _context = context;
        }

        // GET: DatPhong
        public async Task<IActionResult> Index()
        {
            var qlksdbContext = _context.DatPhongs.Include(d => d.MaKhNavigation).Include(d => d.MaPNavigation);
            return View(await qlksdbContext.ToListAsync());
        }

        // GET: DatPhong/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DatPhongs == null)
            {
                return NotFound();
            }

            var datPhongModel = await _context.DatPhongs
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaPNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datPhongModel == null)
            {
                return NotFound();
            }

            return View(datPhongModel);
        }

        // GET: DatPhong/Create
        public IActionResult Create()
        {
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh");
            ViewData["MaP"] = new SelectList(_context.Phongs, "MaP", "MaP");
            return View();
        }

        // POST: DatPhong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaDp,SoNguoi,NgayBatDau,NgayKetThuc,MaKh,MaP")] DatPhongModel datPhongModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datPhongModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", datPhongModel.MaKh);
            ViewData["MaP"] = new SelectList(_context.Phongs, "MaP", "MaP", datPhongModel.MaP);
            return View(datPhongModel);
        }

        // GET: DatPhong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DatPhongs == null)
            {
                return NotFound();
            }

            var datPhongModel = await _context.DatPhongs.FindAsync(id);
            if (datPhongModel == null)
            {
                return NotFound();
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", datPhongModel.MaKh);
            ViewData["MaP"] = new SelectList(_context.Phongs, "MaP", "MaP", datPhongModel.MaP);
            return View(datPhongModel);
        }

        // POST: DatPhong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaDp,SoNguoi,NgayBatDau,NgayKetThuc,MaKh,MaP")] DatPhongModel datPhongModel)
        {
            if (id != datPhongModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datPhongModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatPhongModelExists(datPhongModel.Id))
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
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", datPhongModel.MaKh);
            ViewData["MaP"] = new SelectList(_context.Phongs, "MaP", "MaP", datPhongModel.MaP);
            return View(datPhongModel);
        }

        // GET: DatPhong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DatPhongs == null)
            {
                return NotFound();
            }

            var datPhongModel = await _context.DatPhongs
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaPNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datPhongModel == null)
            {
                return NotFound();
            }

            return View(datPhongModel);
        }

        // POST: DatPhong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DatPhongs == null)
            {
                return Problem("Entity set 'QlksdbContext.DatPhongs'  is null.");
            }
            var datPhongModel = await _context.DatPhongs.FindAsync(id);
            if (datPhongModel != null)
            {
                _context.DatPhongs.Remove(datPhongModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatPhongModelExists(int id)
        {
            return (_context.DatPhongs?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        public async Task<IActionResult> ThanhToanDatPhong(int? id)
        {
            string tenDN = "hoangtamit20";
            var nhanVien = await _context.NhanViens.Include(nv => nv.UserNameNavigation).Where(nv => nv.UserName == tenDN).FirstOrDefaultAsync();
            if (id == null || _context.DatPhongs == null)
            {
                return NotFound();
            }
            var datPhong = await _context.DatPhongs.Include(datPhong => datPhong.MaPNavigation).
                                Include(datPhong => datPhong.MaKhNavigation).
                                Include(datPhong => datPhong.MaPNavigation.MaLpNavigation).
                                Include(datPhong => datPhong.DatDichVus).FirstOrDefaultAsync(dp => dp.Id == id);

            var datDV = await _context.DatDichVus.Include(ddv => ddv.MaDvNavigation).
                                Include(ddv => ddv.MaDpNavigation).
                                Where(ddv => ddv.MaDp == datPhong.MaDp).ToListAsync();

            var hoaDon = await _context.HoaDonDatPhongs.Include(hddp => hddp.MaNvNavigation).Include(hddp => hddp.MaDpNavigation).OrderByDescending(hddp => hddp.MaHddp).ToListAsync();
            if (datPhong == null)
            {
                return NotFound();
            }
            List<object> list = new List<object>();
            list.Add(datPhong);
            list.Add(datDV);
            list.Add(hoaDon);
            list.Add(nhanVien);
            return View(list);
        }




        [HttpPost]
        public async Task<IActionResult> ThanhToanDatPhong(int? Id, string MaHddp, DateTime NgayHd, string MaNv, string MaDp)
        {
            if (Id != null)
            {
                HoaDonDatPhongModel hoaDon = new HoaDonDatPhongModel()
                {
                    MaHddp = MaHddp,
                    NgayHd = NgayHd,
                    MaNv = MaNv,
                    MaDp = MaDp
                };
                if (_context.DatPhongs != null && _context.Phongs != null)
                {
                    var dat = await _context.DatPhongs.Where(dp => dp.MaDp == hoaDon.MaDp).FirstOrDefaultAsync();
                    var datPhong = await _context.DatPhongs.FindAsync(Id);
                    if (datPhong != null)
                    {
                        try
                        {
                            _context.Add(hoaDon);
                            await _context.SaveChangesAsync();
                            var phong = await _context.Phongs.Where(p => p.MaP == datPhong.MaP).FirstOrDefaultAsync();
                            if (phong != null)
                            {
                                try
                                {
                                    phong.TrangThaiPhong = -1; // reset về trạng thái phòng trống
                                    _context.Update(phong);
                                    await _context.SaveChangesAsync();
                                }
                                catch (SqlException ex)
                                {
                                    ModelState.AddModelError("", "Trạng thái phòng không tồn tại!\n" + ex.Message);
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            ModelState.AddModelError("", "Thêm hóa đơn thất bại!\n" + ex.Message);
                        }

                    }
                }
                return RedirectToAction("Index", "HoaDonDatPhong");
            }
            return RedirectToAction("ThanhToanDatPhong", "DatPhong");

        }

        [HttpPost]
        public IActionResult GeneratePDF(string html)
        {
            System.Console.WriteLine(html);
            html = html.Replace("StrTag", "<").Replace("EndTag", ">");
            var htmlToPdf = new HtmlToPdf();
            var pdfDocumnet = htmlToPdf.ConvertHtmlString(html);
            byte[] pdfFile = pdfDocumnet.Save();
            pdfDocumnet.Close();
            return File(pdfFile, "application/pdf", "hoadon.pdf");
        }
    }
}
