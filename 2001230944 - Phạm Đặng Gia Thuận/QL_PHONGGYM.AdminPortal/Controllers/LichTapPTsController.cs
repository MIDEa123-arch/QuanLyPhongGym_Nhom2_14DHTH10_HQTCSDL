using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QL_PHONGGYM.AdminPortal.Data;
using QL_PHONGGYM.AdminPortal.Models;
using System.Data.SqlClient; // QUAN TRỌNG: Để dùng SqlParameter

namespace QL_PHONGGYM.AdminPortal.Controllers
{
    // Chỉ Manager mới được vào trang này
    //[Authorize(Roles = "ManagerRole")]
    public class LichTapPTsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LichTapPTs
        public ActionResult Index()
        {
            // Lấy thêm thông tin PT và Khách hàng để hiển thị cho rõ
            var lichTapPTs = db.LichTapPTs
                .Include(l => l.DangKyPT)
                .Include(l => l.DangKyPT.NhanVien) // Tên PT
                .Include(l => l.DangKyPT.KhachHang); // Tên Khách
            return View(lichTapPTs.ToList());
        }

        // GET: LichTapPTs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichTapPT lichTapPT = db.LichTapPTs
                .Include(l => l.DangKyPT.NhanVien)
                .Include(l => l.DangKyPT.KhachHang)
                .SingleOrDefault(l => l.MaLichPT == id);

            if (lichTapPT == null)
            {
                return HttpNotFound();
            }
            return View(lichTapPT);
        }

        // GET: LichTapPTs/Create
        public ActionResult Create()
        {
            // Dropdown hiển thị chi tiết: Mã - Tên Khách - Tên PT
            var danhSachDangKy = db.DangKyPTs
                .Where(d => d.TrangThai == "Còn hiệu lực" || d.TrangThai == "Chờ duyệt")
                .Select(d => new
                {
                    MaDKPT = d.MaDKPT,
                    ThongTin = "Mã: " + d.MaDKPT + " - KH: " + d.KhachHang.TenKH + " (PT: " + d.NhanVien.TenNV + ")"
                }).ToList();

            ViewBag.MaDKPT = new SelectList(danhSachDangKy, "MaDKPT", "ThongTin");
            return View();
        }

        // POST: LichTapPTs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLichPT,MaDKPT,NgayTap,GioBatDau,GioKetThuc,TrangThai")] LichTapPT lichTapPT)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var pMaDKPT = new SqlParameter("@MaDKPT", lichTapPT.MaDKPT);
                    var pNgayTap = new SqlParameter("@NgayTap", lichTapPT.NgayTap);
                    var pGioBatDau = new SqlParameter("@GioBatDau", lichTapPT.GioBatDau);
                    var pGioKetThuc = new SqlParameter("@GioKetThuc", lichTapPT.GioKetThuc);
                    string trangThaiStr = string.IsNullOrEmpty(lichTapPT.TrangThai) ? "Chưa tập" : lichTapPT.TrangThai;
                    var pTrangThai = new SqlParameter("@TrangThai", trangThaiStr);

                    // Gọi SP Thêm Mới (Có kiểm tra trùng lịch)
                    db.Database.ExecuteSqlCommand("EXEC sp_ThemLichTapPT @MaDKPT, @NgayTap, @GioBatDau, @GioKetThuc, @TrangThai",
                        pMaDKPT, pNgayTap, pGioBatDau, pGioKetThuc, pTrangThai);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string loi = ex.Message;
                    if (ex.InnerException != null) loi = ex.InnerException.InnerException.Message;
                    ModelState.AddModelError("", "KHÔNG THÀNH CÔNG: " + loi);
                }
            }

            // Load lại Dropdown nếu lỗi
            var danhSachDangKy = db.DangKyPTs
                .Where(d => d.TrangThai == "Còn hiệu lực" || d.TrangThai == "Chờ duyệt")
                .Select(d => new { MaDKPT = d.MaDKPT, ThongTin = "Mã: " + d.MaDKPT + " - KH: " + d.KhachHang.TenKH + " (PT: " + d.NhanVien.TenNV + ")" }).ToList();
            ViewBag.MaDKPT = new SelectList(danhSachDangKy, "MaDKPT", "ThongTin", lichTapPT.MaDKPT);
            return View(lichTapPT);
        }

        // GET: LichTapPTs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichTapPT lichTapPT = db.LichTapPTs.Find(id);
            if (lichTapPT == null)
            {
                return HttpNotFound();
            }

            var danhSachDangKy = db.DangKyPTs.Select(d => new
            {
                MaDKPT = d.MaDKPT,
                ThongTin = "Mã: " + d.MaDKPT + " - KH: " + d.KhachHang.TenKH + " (PT: " + d.NhanVien.TenNV + ")"
            }).ToList();

            ViewBag.MaDKPT = new SelectList(danhSachDangKy, "MaDKPT", "ThongTin", lichTapPT.MaDKPT);
            return View(lichTapPT);
        }

        // =====================================================================
        // == HÀM EDIT (POST) ĐÃ ĐƯỢC NÂNG CẤP ĐỂ GỌI SP SỬA ==
        // =====================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLichPT,MaDKPT,NgayTap,GioBatDau,GioKetThuc,TrangThai")] LichTapPT lichTapPT)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var pMaLichPT = new SqlParameter("@MaLichPT", lichTapPT.MaLichPT);
                    var pMaDKPT = new SqlParameter("@MaDKPT", lichTapPT.MaDKPT);
                    var pNgayTap = new SqlParameter("@NgayTap", lichTapPT.NgayTap);
                    var pGioBatDau = new SqlParameter("@GioBatDau", lichTapPT.GioBatDau);
                    var pGioKetThuc = new SqlParameter("@GioKetThuc", lichTapPT.GioKetThuc);
                    string trangThaiStr = string.IsNullOrEmpty(lichTapPT.TrangThai) ? "Chưa tập" : lichTapPT.TrangThai;
                    var pTrangThai = new SqlParameter("@TrangThai", trangThaiStr);

                    // Gọi SP Sửa (Có kiểm tra trùng lịch)
                    db.Database.ExecuteSqlCommand("EXEC sp_SuaLichTapPT @MaLichPT, @MaDKPT, @NgayTap, @GioBatDau, @GioKetThuc, @TrangThai",
                        pMaLichPT, pMaDKPT, pNgayTap, pGioBatDau, pGioKetThuc, pTrangThai);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string loi = ex.Message;
                    if (ex.InnerException != null) loi = ex.InnerException.InnerException.Message;
                    ModelState.AddModelError("", "KHÔNG THÀNH CÔNG: " + loi);
                }
            }

            var danhSachDangKy = db.DangKyPTs.Select(d => new { MaDKPT = d.MaDKPT, ThongTin = "Mã: " + d.MaDKPT + " - KH: " + d.KhachHang.TenKH + " (PT: " + d.NhanVien.TenNV + ")" }).ToList();
            ViewBag.MaDKPT = new SelectList(danhSachDangKy, "MaDKPT", "ThongTin", lichTapPT.MaDKPT);
            return View(lichTapPT);
        }

        // GET: LichTapPTs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Thêm Include
            LichTapPT lichTapPT = db.LichTapPTs
                .Include(l => l.DangKyPT.NhanVien)
                .Include(l => l.DangKyPT.KhachHang)
                .SingleOrDefault(l => l.MaLichPT == id);

            if (lichTapPT == null)
            {
                return HttpNotFound();
            }
            return View(lichTapPT);
        }

        // POST: LichTapPTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LichTapPT lichTapPT = db.LichTapPTs.Find(id);
            db.LichTapPTs.Remove(lichTapPT);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}