using QL_PHONGGYM.Models;
using QL_PHONGGYM.Repositories;
using QL_PHONGGYM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_PHONGGYM.Controllers
{
    public class CartCheckoutController : Controller
    {
        private readonly CartRepository _cartRepo;
        private readonly KhachHangRepository _cusRepo;

        public CartCheckoutController()
        {
            _cartRepo = new CartRepository(new QL_PHONGGYMEntities());
            _cusRepo = new KhachHangRepository(new QL_PHONGGYMEntities());
        }

        

        public ActionResult CheckoutDangKyPT(int maHD, Decimal tongTien, Decimal thanhTien, Decimal giamGia, string url)
        {
            var HoaDonPT = _cartRepo.HoaDonPT(maHD, tongTien, thanhTien, giamGia);            
            var kh = _cusRepo.ThongTinKH((int)Session["maKH"]);

            ViewBag.HoaDon = maHD;
            ViewBag.Khachhang = kh;
            ViewBag.LoaiKh = kh.MaLoaiKH.HasValue ? _cusRepo.LoaiKh(kh.MaLoaiKH.Value) : null;
            @ViewBag.Url = url;
            return View(HoaDonPT);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoHoaDon(FormCollection form)
        {
            var cart = (List<GioHangViewModel>)Session["thanhtoan"];
            int maKH = (int)Session["MaKH"];
            _cartRepo.TaoHoaDon(form, maKH, cart);
            return RedirectToAction("ThanhToanThanhCong");
        }

        public ActionResult ThanhToanfinal()
        {
            int maKH = (int)Session["MaKH"];
            var kh = _cusRepo.ThongTinKH(maKH);

            ViewBag.Khachhang = kh;
            ViewBag.LoaiKh = kh.MaLoaiKH.HasValue ? _cusRepo.LoaiKh(kh.MaLoaiKH.Value) : null;
            ViewBag.DiaChi = Session["Diachi"] as DiaChi;

            var cart = (List<GioHangViewModel>)Session["thanhtoan"];
            return View(cart.OrderByDescending(sp => sp.NgayThem));
        }

        public ActionResult ThanhToanThanhCong()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThanhToanfinal(FormCollection form)
        {
            int maKH = (int)Session["MaKH"];

            _cusRepo.ThemDiaChi(maKH, form);
            Session["Diachi"] = _cusRepo.GetDiaChi(maKH);

            var cart = (List<GioHangViewModel>)Session["thanhtoan"];

            return RedirectToAction("ThanhToanfinal");
        }

        public ActionResult GiamSoLuong(int id)
        {
            _cartRepo.Xoa(id);
            return RedirectToAction("ToCheckOut");

        }

        public ActionResult TangSoLuong(int id)
        {
            _cartRepo.Them(id);
            return RedirectToAction("ToCheckOut");

        }

        public ActionResult XoaDon(int id)
        {
            int maKH = (int)Session["MaKH"];
            _cartRepo.XoaDon(id, maKH);

            return RedirectToAction("ToCheckOut");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaVaThanhToan(FormCollection form, string actionType)
        {
            int maKH = (int)Session["MaKH"];

            if (actionType == "delete")
            {
                _cartRepo.XoaDaChon(form, maKH);
                return RedirectToAction("ToCheckOut");
            }
            else if (actionType == "checkout")
            {

                var list = _cartRepo.ChonSanPham(form, maKH);
                Session["thanhtoan"] = list;
                return RedirectToAction("ThanhToan");
            }

            return RedirectToAction("ToCheckOut");
        }

        public ActionResult ThanhToan()
        {
            int maKH = (int)Session["MaKH"];
            var list = (List<GioHangViewModel>)Session["thanhtoan"];
            var kh = _cusRepo.ThongTinKH(maKH);

            if (kh.MaLoaiKH.HasValue)
            {
                ViewBag.LoaiKh = _cusRepo.LoaiKh(kh.MaLoaiKH.Value);
            }
            else
            {
                ViewBag.LoaiKh = null;
            }
            var diaChi = _cusRepo.GetDiaChi(maKH);
            ViewBag.Khachhang = kh;
            ViewBag.DiaChi = diaChi;
            Session["Diachi"] = diaChi;
            return View(list.OrderByDescending(sp => sp.NgayThem));
        }
        public ActionResult ToCheckOut()
        {
            int maKH = (int)Session["MaKH"];
            var cart = _cartRepo.GetCart(maKH).OrderByDescending(sp => sp.NgayThem);

            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int maSP, int soLuong)
        {
            if (soLuong <= 0)
            {
                TempData["ErrorMessage"] = "Sản phẩm đã hết!";
                return RedirectToAction("ProductDetail", "Product", new { id = maSP });
            }
            if (Session["MaKH"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int maKH = (int)Session["MaKH"];

            try
            {
                bool result = _cartRepo.AddToCart(maKH, maSP, soLuong);
                Session["cart"] = _cartRepo.GetCart(maKH);

                return RedirectToAction("ToCheckOut");
            }
            catch (SqlException ex)
            {
                TempData["ErrorMessage"] = "Lỗi cơ sở dữ liệu: " + ex.Message;
                return RedirectToAction("ProductDetail", "Product", new { id = maSP });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ProductDetail", "Product", new { id = maSP });
            }
        }

    }
}