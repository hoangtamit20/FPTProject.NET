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
    public class TaiKhoanController : Controller
    {
        // public TaiKhoanController(QlksdbContext context) : base(context)
        // {
        // }

        private readonly QlksdbContext _context;

        public TaiKhoanController(QlksdbContext context)
        {
            _context = context;
        }

        // GET: TaiKhoan
        public async Task<IActionResult> Index()
        {
            if (!KiemTraPhanQuyen("Admin"))
            {
                System.Console.WriteLine("đã vào đây!");
                return ChuyenHuong();
            }
            var qlksdbContext = _context.TaiKhoans.Include(t => t.IdRoleNavigation);
            return View(await qlksdbContext.ToListAsync());
        }

        // GET: TaiKhoan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!KiemTraPhanQuyen("Admin"))
            {
                return ChuyenHuong();
            }
            if (id == null || _context.TaiKhoans == null)
            {
                return NotFound();
            }

            var taiKhoanModel = await _context.TaiKhoans
                .Include(t => t.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taiKhoanModel == null)
            {
                return NotFound();
            }

            return View(taiKhoanModel);
        }

        // GET: TaiKhoan/Create
        public IActionResult Create()
        {
            if (!KiemTraPhanQuyen("Admin"))
            {
                return ChuyenHuong();
            }
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole");
            return View();
        }

        // POST: TaiKhoan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,UserPassword,IdRole,Email,PhoneNumber,EmailConfirm,TwoFactorEnabled,TimeLockOut")] TaiKhoanModel taiKhoanModel)
        {

            if (ModelState.IsValid)
            {
                _context.Add(taiKhoanModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", taiKhoanModel.IdRole);
            return View(taiKhoanModel);
        }

        // GET: TaiKhoan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!KiemTraPhanQuyen("Admin"))
            {
                return ChuyenHuong();
            }
            if (id == null || _context.TaiKhoans == null)
            {
                return NotFound();
            }

            var taiKhoanModel = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoanModel == null)
            {
                return NotFound();
            }
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", taiKhoanModel.IdRole);
            return View(taiKhoanModel);
        }

        // POST: TaiKhoan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
        [Bind("Id,UserName,UserPassword,IdRole,Email,PhoneNumber,EmailConfirm,TwoFactorEnabled,TimeLockOut")] TaiKhoanModel taiKhoanModel)
        {
            if (id != taiKhoanModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taiKhoanModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanModelExists(taiKhoanModel.Id))
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
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", taiKhoanModel.IdRole);
            return View(taiKhoanModel);
        }

        // GET: TaiKhoan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!KiemTraPhanQuyen("Admin"))
            {
                return ChuyenHuong();
            }
            if (id == null || _context.TaiKhoans == null)
            {
                return NotFound();
            }

            var taiKhoanModel = await _context.TaiKhoans
                .Include(t => t.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taiKhoanModel == null)
            {
                return NotFound();
            }

            return View(taiKhoanModel);
        }

        // POST: TaiKhoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TaiKhoans == null)
            {
                return Problem("Entity set 'QlksdbContext.TaiKhoans'  is null.");
            }
            var taiKhoanModel = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoanModel != null)
            {
                _context.TaiKhoans.Remove(taiKhoanModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoanModelExists(int id)
        {
            return (_context.TaiKhoans?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public IActionResult DangNhap()
        {
            if (HttpContext.Request.Cookies["DangNhap"] != null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(TaiKhoanModel tkModel)
        {
            //kiểm tra nếu đã đăng nhập thì chuyển sang trang quản lý tài khoản

            if (ModelState.IsValid)
            {
                var check = await _context.TaiKhoans.Where((TaiKhoanModel tk) =>
                    tk.UserName == tkModel.UserName && tk.UserPassword == tkModel.UserPassword).FirstOrDefaultAsync();
                if (check != null)
                {
                    if (check.TwoFactorEnabled)
                    {
                        // hiển thị xác thực 2 lớp

                    }
                    else
                    {

                    }
                    // ViewData["TaiKhoan"] = check;
                    if (check.IdRole == -1) // TH: tài khoản bị khóa
                    {
                        ModelState.AddModelError("name", "Tài khoản của bạn đã bị khóa vui lòng liên hệ admin để được hỗ trợ!");
                        return View(tkModel);
                    }
                    else
                    {
                        string cookieValue = check.UserName + ";" + check.IdRole;
                        BaseClass bs = new BaseClass();
                        cookieValue = bs.hashString(cookieValue);
                        HttpContext.Response.Cookies.Append("DangNhap", cookieValue, new CookieOptions
                        {
                            Expires = DateTime.Now.AddMonths(1),
                            Path = "/"
                        });
                        if (check.IdRole == 99) // tài khoản admin
                        {
                            return RedirectToAction("Index", "TaiKhoan");
                        }
                        else if (check.IdRole == 0) // tài khoản nhân viên
                        {
                            // redirect về trang đặt phòng
                            return RedirectToAction("Index", "DatPhong");
                        }
                        else if (check.IdRole == 1) // tài khoản quản lý
                        {
                            //redirect về trang quản lý nhân viên
                            return RedirectToAction("Index", "NhanVien");
                        }
                    }
                }
                else
                {
                    // System.Console.WriteLine("bị null rồi!");
                    ModelState.AddModelError("name", "Sai tên đăng nhập hoặc mật khẩu!");
                    return View(tkModel);
                }

            }
            return View();
        }

        public IActionResult DangXuat()
        {
            if (HttpContext.Request.Cookies["DangNhap"] != null)
            {
                // HttpContext.Response.Cookies.Append("DangNhap", "", new CookieOptions{
                //     Expires = DateTime.Now.AddDays(-1)
                // });
                foreach (var cookie in HttpContext.Request.Cookies)
                {
                    if (cookie.Key.Equals("DangNhap"))
                    {
                        Response.Cookies.Delete(cookie.Key);
                    }

                }
                return RedirectToAction(nameof(DangNhap));
            }
            return View();
        }


        public IActionResult QuenMatKhau()
        {
            if (KiemTraPhanQuyen(null) == true)
            {
                return ChuyenHuong();
            }
            return View();
            // string? strCookie = HttpContext.Request.Cookies["DangNhap"];
            // if (strCookie != null)
            // {
            //     BaseClass bs = new BaseClass();
            //     strCookie = bs.deCodeHash(strCookie);
            //     try
            //     {
            //         int so = Int32.Parse(strCookie.Split(";")[1]);
            //         if (so == 99)
            //         {
            //             return RedirectToAction(nameof(Index));
            //         }
            //         else if (so == 1)
            //         {
            //             // return RedirectToAction("Index", "NhanVien");
            //             return RedirectToAction(nameof(Index));
            //         }
            //         else if (so == 0)
            //         {

            //             // return RedirectToAction("DatPhong","Phong");
            //             return RedirectToAction(nameof(Index));

            //         }
            //         else
            //         {
            //             // return RedirectToAction("AccessDenied");
            //             return RedirectToAction(nameof(Index));
            //         }
            //     }
            //     catch (FormatException ex)
            //     {
            //         ModelState.AddModelError("name", "Lỗi đọc cookie : " + ex.Message);
            //     }
            // }
            // return View();
        }


        [HttpPost]
        public async Task<IActionResult> QuenMatKhau(string email)
        {
            if (_context.TaiKhoans != null)
            {
                var tk = await _context.TaiKhoans.Where(tk => tk.Email == email).FirstOrDefaultAsync();
                // var tk = (from t in _context.TaiKhoans.ToList() where t.Email == email select t).FirstOrDefault();
                string strCodeEmail = "";
                if (tk != null)
                {
                    TempData["PassModelResetPass"] = tk.UserName;
                    //tiến hành gửi email và và gửi mã code lên ViewData
                    BaseClass bs = new BaseClass();
                    strCodeEmail = bs.randEmailCode();
                    TempData["strCodeEmail"] = strCodeEmail;
                    if (bs.sendEmailGetCode(strCodeEmail, email) == true)
                    {
                        return RedirectToAction(nameof(XacThucMail));
                    }
                    else
                    {
                        // show thong bao gui mail that bai!
                        ModelState.AddModelError("name", "Gửi email thất bại!");
                        // System.Console.WriteLine("That bai!");
                    }
                }
                else
                {
                    // show thong bao email khong ton tai
                    ModelState.AddModelError("", "Email không tồn tại!");
                    TempData["PassModelResetPass"] = null;
                    // System.Console.WriteLine("Da vao day!");
                }
            }
            return View();
        }


        public IActionResult XacThucMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> XacThucMail(string code)
        {
            string? strCodeEmail = "";
            if (TempData["strCodeEmail"] != null)
            {
                strCodeEmail = TempData["strCodeEmail"].ToString();
            }
            if (code.Equals(strCodeEmail))
            {
                // sang trang đổi mật khẩu
                return RedirectToAction(nameof(DoiMatKhau));

            }
            ModelState.AddModelError("name", "Code không khớp!");
            //thông báo lỗi
            return View(code);
        }


        public IActionResult DoiMatKhau()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DoiMatKhau(string pass, string repass)
        {
            if (pass == repass)
            {
                string? tenDN = "";
                if (TempData["PassModelResetPass"] != null)
                {
                    tenDN = TempData["PassModelResetPass"].ToString();
                    var tkResetPass = _context.TaiKhoans.Where(tkModel => tkModel.UserName == tenDN).FirstOrDefault();
                    if (tkResetPass != null)
                    {
                        tkResetPass.UserPassword = pass;
                        _context.Update(tkResetPass);
                        await _context.SaveChangesAsync();
                        // Thong bao doi mat khau thanh cong!x`
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đổi mật khẩu thất bại!");
                        // Thong bao doi mat khau that bai
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Mật khẩu không khớp!");
                // thong bao mat khau khong khop
            }
            return View();
        }

        // public IActionResult ThongKe()
        // {
        //     return View();
        // }


        public bool KiemTraPhanQuyen(string roleName)
        {
            // bool result = false;
            int roleId = -99;
            string? strCookie = HttpContext.Request.Cookies["DangNhap"] == null ? "" : HttpContext.Request.Cookies["DangNhap"];
            if (!string.IsNullOrEmpty(strCookie) && _context.TaiKhoans != null)
            {
                BaseClass bs = new BaseClass();
                strCookie = bs.deCodeHash(strCookie);
                string tenDN = strCookie.Split(";")[0];
                Int32.TryParse(strCookie.Split(";")[1], out roleId);
                if (_context.TaiKhoans.Where(tk => tk.UserName == tenDN).FirstOrDefault() != null) // đã đăng nhập
                {
                    if (!string.IsNullOrEmpty(roleName)) // trường sử dụng phân quyền để truy cập
                    {
                        if (roleName.Equals("Admin") && roleId == 99)
                        {
                            // System.Console.WriteLine("Anh vao day roi ne!");
                            return true;
                        }
                        else if (roleName.Equals("QuanLy") && roleId == 1)
                        {
                            // System.Console.WriteLine("Anh vao day roi ne 1!");
                            return true;
                        }
                        else if (roleName.Equals("NhanVien") && roleId == 0)
                        {
                            // System.Console.WriteLine("Anh vao day roi ne 2!");
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else // trường hợp xác nhận đăng nhập
                    {
                        return true;
                    }
                }
                else
                {
                    // cookie vẫn được lưu nhưng tài khoản trong database đã bị xóa -> tiến hành xóa cookie
                    xoaCookieDangNhap();
                    ModelState.AddModelError("", "Tài khoản đã không còn tồn tại trên hệ thống! Cookie đã được xóa bỏ!");
                }

                return true;
            }
            else
            {
                return false;
            }

        }

        public void xoaCookieDangNhap()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                if (cookie.Key.Equals("DangNhap"))
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }
        }


        public IActionResult ChuyenHuong()
        {
            BaseClass bs = new BaseClass();
            int roleId = Int32.Parse(bs.deCodeHash(HttpContext.Request.Cookies["DangNhap"]).Split(";")[1]);
            System.Console.WriteLine(roleId);
            if (roleId == 99)
            {
                return RedirectToAction("Index", "TaiKhoan");
            }
            else if (roleId == 1)
            {
                return RedirectToAction("Index", "NhanVien");
            }
            else if (roleId == 0)
            {
                return RedirectToAction("Index", "DatPhong");
            }
            return NotFound();
        }
    }
}
