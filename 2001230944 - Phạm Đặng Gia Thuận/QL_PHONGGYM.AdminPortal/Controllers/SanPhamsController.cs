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
    // Bảo vệ trang (Chỉ Manager mới được vào)
    //[Authorize(Roles = "ManagerRole")]
    public class SanPhamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SanPhams
        public ActionResult Index()
        {
            var sanPhams = db.SanPhams.Include(s => s.LoaiSanPham);
            return View(sanPhams.ToList());
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Include HINHANHs để hiển thị Gallery ảnh
            SanPham sanPham = db.SanPhams
                                .Include(s => s.LoaiSanPham)
                                .Include(s => s.HINHANHs)
                                .SingleOrDefault(s => s.MaSP == id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoaiSP");
            return View();
        }

        // ============================================================
        // == HÀM CREATE (POST) GỌI SP ĐỂ CHẶN TRÙNG SẢN PHẨM ==
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,MaLoaiSP,DonGia,SoLuongTon,GiaKhuyenMai,Hang,XuatXu,BaoHanh,MoTaChung,MoTaChiTiet")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Chuẩn bị tham số
                    var pTenSP = new SqlParameter("@TenSP", sanPham.TenSP);
                    var pMaLoaiSP = new SqlParameter("@MaLoaiSP", sanPham.MaLoaiSP);
                    var pDonGia = new SqlParameter("@DonGia", sanPham.DonGia);
                    var pSoLuongTon = new SqlParameter("@SoLuongTon", sanPham.SoLuongTon);

                    // Các tham số có thể Null (dùng DBNull.Value)
                    var pGiaKhuyenMai = new SqlParameter("@GiaKhuyenMai", (object)sanPham.GiaKhuyenMai ?? DBNull.Value);
                    var pHang = new SqlParameter("@Hang", (object)sanPham.Hang ?? DBNull.Value);
                    var pXuatXu = new SqlParameter("@XuatXu", (object)sanPham.XuatXu ?? DBNull.Value);
                    var pBaoHanh = new SqlParameter("@BaoHanh", (object)sanPham.BaoHanh ?? DBNull.Value);
                    var pMoTaChung = new SqlParameter("@MoTaChung", (object)sanPham.MoTaChung ?? DBNull.Value);
                    var pMoTaChiTiet = new SqlParameter("@MoTaChiTiet", (object)sanPham.MoTaChiTiet ?? DBNull.Value);

                    // 2. Gọi Stored Procedure
                    db.Database.ExecuteSqlCommand(
                        "EXEC sp_ThemSanPham @TenSP, @MaLoaiSP, @DonGia, @SoLuongTon, @GiaKhuyenMai, @Hang, @XuatXu, @BaoHanh, @MoTaChung, @MoTaChiTiet",
                        pTenSP, pMaLoaiSP, pDonGia, pSoLuongTon, pGiaKhuyenMai, pHang, pXuatXu, pBaoHanh, pMoTaChung, pMoTaChiTiet
                    );

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // 3. Bắt lỗi trùng lặp từ SQL (RAISERROR)
                    string loiNhanDuoc = ex.Message;
                    if (ex.InnerException != null && ex.InnerException.InnerException != null)
                    {
                        loiNhanDuoc = ex.InnerException.InnerException.Message;
                    }
                    ModelState.AddModelError("", "KHÔNG THÀNH CÔNG: " + loiNhanDuoc);
                }
            }

            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Include HINHANHs để trang Edit có thể quản lý ảnh (bước sau)
            SanPham sanPham = db.SanPhams
                                .Include(s => s.HINHANHs)
                                .SingleOrDefault(s => s.MaSP == id);

            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }

        // ============================================================
        // == HÀM EDIT (POST) GỌI SP ĐỂ CHẶN TRÙNG KHI SỬA ==
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,MaLoaiSP,DonGia,SoLuongTon,GiaKhuyenMai,Hang,XuatXu,BaoHanh,MoTaChung,MoTaChiTiet")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Chuẩn bị tham số (Bao gồm cả MaSP)
                    var pMaSP = new SqlParameter("@MaSP", sanPham.MaSP);
                    var pTenSP = new SqlParameter("@TenSP", sanPham.TenSP);
                    var pMaLoaiSP = new SqlParameter("@MaLoaiSP", sanPham.MaLoaiSP);
                    var pDonGia = new SqlParameter("@DonGia", sanPham.DonGia);
                    var pSoLuongTon = new SqlParameter("@SoLuongTon", sanPham.SoLuongTon);

                    var pGiaKhuyenMai = new SqlParameter("@GiaKhuyenMai", (object)sanPham.GiaKhuyenMai ?? DBNull.Value);
                    var pHang = new SqlParameter("@Hang", (object)sanPham.Hang ?? DBNull.Value);
                    var pXuatXu = new SqlParameter("@XuatXu", (object)sanPham.XuatXu ?? DBNull.Value);
                    var pBaoHanh = new SqlParameter("@BaoHanh", (object)sanPham.BaoHanh ?? DBNull.Value);
                    var pMoTaChung = new SqlParameter("@MoTaChung", (object)sanPham.MoTaChung ?? DBNull.Value);
                    var pMoTaChiTiet = new SqlParameter("@MoTaChiTiet", (object)sanPham.MoTaChiTiet ?? DBNull.Value);

                    // 2. Gọi Stored Procedure SỬA (Có kiểm tra trùng)
                    // (Bạn nhớ chạy SQL Script tạo sp_SuaSanPham trước nhé!)
                    db.Database.ExecuteSqlCommand(
                        "EXEC sp_SuaSanPham @MaSP, @TenSP, @MaLoaiSP, @DonGia, @SoLuongTon, @GiaKhuyenMai, @Hang, @XuatXu, @BaoHanh, @MoTaChung, @MoTaChiTiet",
                        pMaSP, pTenSP, pMaLoaiSP, pDonGia, pSoLuongTon, pGiaKhuyenMai, pHang, pXuatXu, pBaoHanh, pMoTaChung, pMoTaChiTiet
                    );

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // 3. Bắt lỗi trùng lặp từ SQL
                    string loiNhanDuoc = ex.Message;
                    if (ex.InnerException != null && ex.InnerException.InnerException != null)
                    {
                        loiNhanDuoc = ex.InnerException.InnerException.Message;
                    }
                    ModelState.AddModelError("", "KHÔNG THÀNH CÔNG: " + loiNhanDuoc);
                }
            }

            // Nếu lỗi, tải lại Dropdown
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Include(s => s.LoaiSanPham).SingleOrDefault(s => s.MaSP == id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
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