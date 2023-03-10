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

        public void taoMaDp()
        {
            var datP = _context.DatPhongs.OrderByDescending(dp => dp.Id).FirstOrDefault();
            if (datP != null)
            {
                ViewData["MaDp"] = "MDP" + datP.Id.ToString();
            }
            else
            {
                ViewData["MaDp"] = "MDP1";
            }
        }

        // GET: DatPhong
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var listP = await _context.Phongs
                .Include(p => p.MaLpNavigation)
                .OrderByDescending(p => p.SoPhong)
                .ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                listP = listP
                    .Where(
                        s =>
                            s.SoPhong.ToString().Contains(searchString)
                            || s.MaLpNavigation.TenLp.Contains(searchString)
                    )
                    .ToList();
            }
            var datPhong = (
                from dp in _context.DatPhongs
                join hddp in _context.HoaDonDatPhongs on dp.MaDp equals hddp.MaDp into g
                from u in g.DefaultIfEmpty()
                select new Models.DatPhongModel
                {
                    Id = dp.Id,
                    MaP = dp.MaP,
                    MaDp = dp.MaDp
                }
            ).ToList();
            var listobject = new List<object>();
            listobject.Add(listP);
            listobject.Add(datPhong);
            return View(listobject);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int? id)
        {
            //1 l?? r???nh, 0 l?? ???? ?????t, 2 l?? t???t c???
            var datPhong = (
                from dp in _context.DatPhongs
                join hddp in _context.HoaDonDatPhongs on dp.MaDp equals hddp.MaDp into g
                from u in g.DefaultIfEmpty()
                select new Models.DatPhongModel
                {
                    Id = dp.Id,
                    MaP = dp.MaP,
                    MaDp = dp.MaDp
                }
            ).ToList();
            var listobject = new List<object>();
            if (id == 0)
            {
                ViewData["checkNav"] = 0;
                var test = await _context.Phongs
                        .Where(p => p.TrangThaiPhong == 0)
                        .Include(p => p.MaLpNavigation)
                        .OrderByDescending(p => p.SoPhong)
                        .ToListAsync();
                listobject.Add(test);
            }
            else if (id == 1)
            {
                ViewData["checkNav"] = 1;
                listobject.Add(
                    await _context.Phongs
                        .Where(p => p.TrangThaiPhong == 1)
                        .Include(p => p.MaLpNavigation)
                        .OrderByDescending(p => p.SoPhong)
                        .ToListAsync()
                );
            }
            else
            {
                ViewData["checkNav"] = 2;
                var listP = await _context.Phongs
                    .Include(p => p.MaLpNavigation)
                    .Include(p => p.MaLpNavigation)
                    .OrderByDescending(p => p.SoPhong)
                    .ToListAsync();
                listobject.Add(listP);
            }
            listobject.Add(datPhong);
            return View(listobject);
        }

        // GET: DatPhong/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DatPhongs == null)
            {
                return NotFound();
            }

            var datphong = await _context.DatPhongs
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaPNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datphong == null)
            {
                return NotFound();
            }

            return View(datphong);
        }

        // GET: DatPhong/Create
        [HttpGet]
        public IActionResult Create(int? idP)
        {
            var phong = _context.Phongs.FirstOrDefault(p => p.Id == idP);
            ViewData["MaP"] = phong.MaP;
            ViewData["SoPhong"] = phong.SoPhong;
            ViewData["IdP"] = idP;
            taoMaDp();
            return View();
        }

        // POST: DatPhong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int? idP,
            [Bind("Id,MaDp,SoNguoi,NgayBatDau,NgayKetThuc,MaP,MaKh")] DatPhongModel datphong,
            [Bind("Id, MaKh, TenKh")] KhachHangModel khachhang
        )
        {
            var khachHangs = await _context.KhachHangs
                .OrderByDescending(kh => kh.Id)
                .FirstOrDefaultAsync();
            var makh = "KH" + (khachHangs.Id + 1).ToString();
            System.Console.WriteLine(makh);
            datphong.MaKh = makh;
            khachhang.MaKh = makh;
            if (ModelState.IsValid)
            {
                _context.Add(khachhang);
                await _context.SaveChangesAsync();
                _context.Add(datphong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // ViewData["MaP"] = new SelectList(_context.Phongs, "MaP", "SoPhong", datphong.MaP);
            return View(datphong);
        }

        // GET: DatPhong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //nv so 0, ql 1, admin 99, -1 bi khoa -> access denied
            // string tenDN = HttpContext.Request.Cookies["cookieName"];
            if (HttpContext.Request.Cookies["cookieName"] != null) { }
            if (id == null || _context.DatPhongs == null)
            {
                return NotFound();
            }

            var datphong = await _context.DatPhongs.FindAsync(id);
            if (datphong == null)
            {
                return NotFound();
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", datphong.MaKh);
            ViewData["MaP"] = new SelectList(_context.Phongs, "MaP", "MaP", datphong.MaP);
            return View(datphong);
        }

        // POST: DatPhong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,MaDp,SoNguoi,ThoiGianBatDau,ThoiGianKetThuc,MaP,MaKh")] DatPhongModel datphong
        )
        {
            if (id != datphong.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datphong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatphongExists(datphong.Id))
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
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", datphong.MaKh);
            ViewData["MaP"] = new SelectList(_context.Phongs, "MaP", "MaP", datphong.MaP);
            return View(datphong);
        }

        // GET: DatPhong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DatPhongs == null)
            {
                return NotFound();
            }

            var datphong = await _context.DatPhongs
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaPNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datphong == null)
            {
                return NotFound();
            }

            return View(datphong);
        }

        // POST: DatPhong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DatPhongs == null)
            {
                return Problem("Entity set 'QlksContext.DatPhongs'  is null.");
            }
            var datphong = await _context.DatPhongs.FindAsync(id);
            if (datphong != null)
            {
                _context.DatPhongs.Remove(datphong);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatphongExists(int id)
        {
            return (_context.DatPhongs?.Any(e => e.Id == id)).GetValueOrDefault();
        }




        public async Task<IActionResult> ThanhToanDatPhong(int? idDP)
        {
            string tenDN = "hoangtamit20";
            var nhanVien = await _context.NhanViens.Include(nv => nv.UserNameNavigation).Where(nv => nv.UserName == tenDN).FirstOrDefaultAsync();
            if (idDP == null || _context.DatPhongs == null)
            {
                return NotFound();
            }
            var datPhong = await _context.DatPhongs.Include(datPhong => datPhong.MaPNavigation).
                                Include(datPhong => datPhong.MaKhNavigation).
                                Include(datPhong => datPhong.MaPNavigation.MaLpNavigation).
                                Include(datPhong => datPhong.DatDichVus).FirstOrDefaultAsync(dp => dp.Id == idDP);

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



        // string MaHddp, DateTime NgayHd, string MaNv, string MaDp
        [HttpPost]
        public async Task<IActionResult> ThanhToanDatPhong(int? Id, HoaDonDatPhongModel hoaDon)
        {
            HoaDonDatPhongModel hoaDonDatPhong = new HoaDonDatPhongModel(){
                MaHddp = hoaDon.MaHddp,
                NgayHd = hoaDon.NgayHd,
                MaNv = hoaDon.MaNv,
                MaDp = hoaDon.MaDp
            };
            if (Id != null)
            {
                if (_context.DatPhongs != null && _context.Phongs != null)
                {
                    var dat = await _context.DatPhongs.Where(dp => dp.MaDp == hoaDonDatPhong.MaDp).FirstOrDefaultAsync();
                    var datPhong = await _context.DatPhongs.FindAsync(Id);
                    if (datPhong != null)
                    {
                        try
                        {
                            _context.Add(hoaDonDatPhong);
                            await _context.SaveChangesAsync();
                            var phong = await _context.Phongs.Where(p => p.MaP == datPhong.MaP).FirstOrDefaultAsync();
                            if (phong != null)
                            {
                                try
                                {
                                    phong.TrangThaiPhong = 0; // reset v??? tr???ng th??i ph??ng tr???ng
                                    _context.Update(phong);
                                    await _context.SaveChangesAsync();
                                }
                                catch (SqlException ex)
                                {
                                    ModelState.AddModelError("", "Tr???ng th??i ph??ng kh??ng t???n t???i!\n" + ex.Message);
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            ModelState.AddModelError("", "Th??m h??a ????n th???t b???i!\n" + ex.Message);
                        }
                    }
                }
                return RedirectToAction("Index", "HoaDonDatPhong");
            }
            return RedirectToAction("ThanhToanDatPhong","DatPhong", new {@idDP=Id});
        }

        [HttpPost]
        public IActionResult GeneratePDF(string html)
        {
            // System.Console.WriteLine(html);
            html = html.Replace("StrTag", "<").Replace("EndTag", ">");
            var htmlToPdf = new HtmlToPdf();
            var pdfDocumnet = htmlToPdf.ConvertHtmlString(html);
            byte[] pdfFile = pdfDocumnet.Save();
            pdfDocumnet.Close();
            return File(pdfFile, "application/pdf", "hoadon.pdf");
        }
    }
}
