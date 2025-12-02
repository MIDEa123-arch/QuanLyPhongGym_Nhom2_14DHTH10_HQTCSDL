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

namespace QL_PHONGGYM.AdminPortal.Controllers
{
    // Vẫn giữ bảo vệ Role Manager
   // [Authorize(Roles = "ManagerRole")]
    public class NhanViensController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NhanViens
        public ActionResult Index(string searchString)
        {
            // 1. Lấy danh sách nhân viên ban đầu (chưa chạy câu lệnh SQL)
            var nhanViens = db.NhanViens.Include(n => n.ChucVu).AsQueryable();

            // 2. Nếu có từ khóa tìm kiếm -> Thêm điều kiện lọc
            if (!String.IsNullOrEmpty(searchString))
            {
                // Tìm theo Tên Nhân Viên (chứa từ khóa)
                // Bạn có thể thêm || n.SDT.Contains(searchString) để tìm cả theo SĐT
                nhanViens = nhanViens.Where(n => n.TenNV.Contains(searchString));
            }

            // 3. Sắp xếp và trả về danh sách
            return View(nhanViens.OrderBy(n => n.TenNV).ToList());
            // Chỉ lấy Nhân viên và Chức vụ, không tính toán đếm khách
            // Thêm .OrderBy(n => n.TenNV) để sắp xếp tên A-Z cho dễ nhìn
            
        }

        // GET: NhanViens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens
                .Include(n => n.ChucVu)
                .Include(n => n.NhanVienChuyenMons.Select(c => c.ChuyenMon)) // Vẫn lấy chuyên môn để xem chi tiết
                .SingleOrDefault(n => n.MaNV == id);

            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public ActionResult Create()
        {
            ViewBag.MaChucVu = new SelectList(db.ChucVus, "MaChucVu", "TenChucVu");
            return View();
        }

        // POST: NhanViens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,MaChucVu,TenDangNhap,MatKhau,GioiTinh,TenNV,SDT,NgaySinh")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.NhanViens.Add(nhanVien);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string loi = ex.Message;
                    if (ex.InnerException != null) loi = ex.InnerException.InnerException.Message;

                    // Hiển thị lỗi của Trigger lên màn hình web
                    ModelState.AddModelError("", "LỖI TỪ CSDL: " + loi);
                }
                
            }

            ViewBag.MaChucVu = new SelectList(db.ChucVus, "MaChucVu", "TenChucVu", nhanVien.MaChucVu);
            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaChucVu = new SelectList(db.ChucVus, "MaChucVu", "TenChucVu", nhanVien.MaChucVu);
            return View(nhanVien);
        }

        // POST: NhanViens/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,MaChucVu,TenDangNhap,MatKhau,GioiTinh,TenNV,SDT,NgaySinh")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaChucVu = new SelectList(db.ChucVus, "MaChucVu", "TenChucVu", nhanVien.MaChucVu);
            return View(nhanVien);
        }

        // GET: NhanViens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Include(n => n.ChucVu).SingleOrDefault(n => n.MaNV == id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // Gọi Stored Procedure "sp_XoaNhanVien" để xóa an toàn
                var pMaNV = new System.Data.SqlClient.SqlParameter("@MaNV", id);
                db.Database.ExecuteSqlCommand("EXEC sp_XoaNhanVien @MaNV", pMaNV);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Lấy thông báo lỗi chi tiết
                string loi = ex.Message;
                if (ex.InnerException != null) loi = ex.InnerException.InnerException.Message;

                // Gửi lỗi ra View để hiển thị trong @Html.ValidationSummary
                ModelState.AddModelError("", "LỖI KHI XÓA: " + loi);

                // Trả về trang xóa để người dùng đọc lỗi
                NhanVien nhanVien = db.NhanViens.Include(n => n.ChucVu).SingleOrDefault(n => n.MaNV == id);
                return View(nhanVien);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}