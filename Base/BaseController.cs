using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hotel.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SelectPdf;

namespace Hotel.Base
{
    public class BaseController : Controller
    {
        protected readonly QlksdbContext _context;
        public BaseController(QlksdbContext context)
        {
            _context = context;
        }

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
                            return true;
                        }
                        else if (roleName.Equals("QuanLy") && roleId == 1)
                        {
                            return true;
                        }
                        else if (roleName.Equals("NhanVien") && roleId == 0)
                        {
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
                System.Console.WriteLine("Da vao day!");
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
            if (HttpContext.Request.Cookies["DangNhap"] != null)
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
            else
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
        }
    }
}