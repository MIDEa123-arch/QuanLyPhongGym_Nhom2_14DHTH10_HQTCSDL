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
using System.Data.SqlClient;

namespace QL_PHONGGYM.AdminPortal.Controllers
{
    //[Authorize(Roles = "ManagerRole")]
    public class LichLopsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LichLop
        public ActionResult Index()
        {
            var lichLops = db.LichLops
                .Include(l => l.LopHoc)
                .Include(l => l.NhanVien)
                .OrderBy(l => l.NgayHoc)
                .ThenBy(l => l.LopHoc.TenLop);
            return View(lichLops.ToList());
        }

        // GET: LichLop/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichLop lichLop = db.LichLops.Include(l => l.LopHoc).Include(l => l.NhanVien).SingleOrDefault(l => l.MaLichLop == id);
            if (lichLop == null)
            {
                return HttpNotFound();
            }
            return View(lichLop);
        }

        // ============================================================
        // == HÀM HELPER: TẠO DROPDOWN LỚP HỌC KÈM TÊN GIÁO VIÊN ==
        // ============================================================
        private SelectList GetLopHocSelectList(object selectedValue = null, bool onlyActive = true)
        {
            var query = db.LopHocs.Include(l => l.NhanVien).AsQueryable();

            // Nếu là trang Create, chỉ lấy lớp chưa kết thúc
            if (onlyActive)
            {
                query = query.Where(l => l.NgayKetThuc >= DateTime.Today);
            }

            var listLopHoc = query.ToList()
                .Select(l => new
                {
                    MaLop = l.MaLop,
                    // ĐÂY LÀ CHỖ HIỂN THỊ TÊN GV: "Yoga (GV: Nguyễn Văn A)"
                    TenHienThi = l.TenLop + (l.NhanVien != null ? $" (GV: {l.NhanVien.TenNV})" : " (Chưa có GV)")
                });

            return new SelectList(listLopHoc, "MaLop", "TenHienThi", selectedValue);
        }
        // ============================================================

        // GET: LichLop/Create
        public ActionResult Create()
        {
            // Sử dụng hàm Helper để lấy danh sách lớp ĐẸP
            ViewBag.MaLop = GetLopHocSelectList(null, true);

            var giaoViens = db.NhanViens
                .Where(n => n.ChucVu.TenChucVu == "HLV Lớp" || n.ChucVu.TenChucVu == "PT")
                .Select(n => new { MaNV = (int?)n.MaNV, TenNV = n.TenNV }).ToList();
            giaoViens.Insert(0, new { MaNV = (int?)null, TenNV = "--- Mặc định (Theo Lớp) ---" });

            ViewBag.MaNV = new SelectList(giaoViens, "MaNV", "TenNV");
            return View();
        }

        // POST: LichLop/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLichLop,MaLop,MaNV,NgayHoc,GioBatDau,GioKetThuc")] LichLop lichLop)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var pMaLop = new SqlParameter("@MaLop", lichLop.MaLop);
                    var pMaNV = new SqlParameter("@MaNV", (object)lichLop.MaNV ?? DBNull.Value);
                    var pNgayHoc = new SqlParameter("@NgayHoc", lichLop.NgayHoc);
                    var pGioBatDau = new SqlParameter("@GioBatDau", lichLop.GioBatDau);
                    var pGioKetThuc = new SqlParameter("@GioKetThuc", lichLop.GioKetThuc);

                    db.Database.ExecuteSqlCommand("EXEC sp_ThemLichLop @MaLop, @MaNV, @NgayHoc, @GioBatDau, @GioKetThuc",
                        pMaLop, pMaNV, pNgayHoc, pGioBatDau, pGioKetThuc);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string loi = ex.Message;
                    if (ex.InnerException != null) loi = ex.InnerException.InnerException.Message;
                    ModelState.AddModelError("", "LỖI: " + loi);
                }
            }

            // Nếu lỗi, load lại danh sách lớp ĐẸP
            ViewBag.MaLop = GetLopHocSelectList(lichLop.MaLop, true);

            var giaoViens = db.NhanViens.Where(n => n.ChucVu.TenChucVu == "HLV Lớp" || n.ChucVu.TenChucVu == "PT")
                                        .Select(n => new { MaNV = (int?)n.MaNV, TenNV = n.TenNV }).ToList();
            giaoViens.Insert(0, new { MaNV = (int?)null, TenNV = "--- Mặc định (Theo Lớp) ---" });
            ViewBag.MaNV = new SelectList(giaoViens, "MaNV", "TenNV", lichLop.MaNV);

            return View(lichLop);
        }

        // GET: LichLop/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            LichLop lichLop = db.LichLops.Find(id);
            if (lichLop == null) return HttpNotFound();

            // Load danh sách lớp ĐẸP cho Edit (không lọc ngày kết thúc để tránh lỗi hiển thị lịch sử)
            ViewBag.MaLop = GetLopHocSelectList(lichLop.MaLop, false);

            var giaoViens = db.NhanViens.Where(n => n.ChucVu.TenChucVu == "HLV Lớp" || n.ChucVu.TenChucVu == "PT")
                                        .Select(n => new { MaNV = (int?)n.MaNV, TenNV = n.TenNV }).ToList();
            giaoViens.Insert(0, new { MaNV = (int?)null, TenNV = "--- Mặc định (Theo Lớp) ---" });

            ViewBag.MaNV = new SelectList(giaoViens, "MaNV", "TenNV", lichLop.MaNV);
            return View(lichLop);
        }

        // POST: LichLop/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLichLop,MaLop,MaNV,NgayHoc,GioBatDau,GioKetThuc")] LichLop lichLop)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var pMaLichLop = new SqlParameter("@MaLichLop", lichLop.MaLichLop);
                    var pMaLop = new SqlParameter("@MaLop", lichLop.MaLop);
                    var pMaNV = new SqlParameter("@MaNV", (object)lichLop.MaNV ?? DBNull.Value);
                    var pNgayHoc = new SqlParameter("@NgayHoc", lichLop.NgayHoc);
                    var pGioBatDau = new SqlParameter("@GioBatDau", lichLop.GioBatDau);
                    var pGioKetThuc = new SqlParameter("@GioKetThuc", lichLop.GioKetThuc);

                    db.Database.ExecuteSqlCommand("EXEC sp_SuaLichLop @MaLichLop, @MaLop, @MaNV, @NgayHoc, @GioBatDau, @GioKetThuc",
                        pMaLichLop, pMaLop, pMaNV, pNgayHoc, pGioBatDau, pGioKetThuc);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string loi = ex.Message;
                    if (ex.InnerException != null) loi = ex.InnerException.InnerException.Message;
                    ModelState.AddModelError("", "LỖI: " + loi);
                }
            }

            // Nếu lỗi, load lại danh sách lớp ĐẸP
            ViewBag.MaLop = GetLopHocSelectList(lichLop.MaLop, false);

            var giaoViens = db.NhanViens.Where(n => n.ChucVu.TenChucVu == "HLV Lớp" || n.ChucVu.TenChucVu == "PT")
                                        .Select(n => new { MaNV = (int?)n.MaNV, TenNV = n.TenNV }).ToList();
            giaoViens.Insert(0, new { MaNV = (int?)null, TenNV = "--- Mặc định (Theo Lớp) ---" });
            ViewBag.MaNV = new SelectList(giaoViens, "MaNV", "TenNV", lichLop.MaNV);

            return View(lichLop);
        }

        // GET: LichLop/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            LichLop lichLop = db.LichLops.Include(l => l.LopHoc).Include(l => l.NhanVien).SingleOrDefault(l => l.MaLichLop == id);
            if (lichLop == null) return HttpNotFound();
            return View(lichLop);
        }

        // POST: LichLop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LichLop lichLop = db.LichLops.Find(id);
            db.LichLops.Remove(lichLop);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}