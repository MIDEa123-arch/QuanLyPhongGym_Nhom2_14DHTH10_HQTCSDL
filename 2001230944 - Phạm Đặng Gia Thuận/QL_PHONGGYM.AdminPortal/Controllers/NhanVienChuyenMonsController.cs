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
    // 1. ĐÃ THÊM [Authorize]
    // Nó sẽ hoạt động 100% vì chúng ta đã sửa Global.asax.cs
    
    public class NhanVienChuyenMonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NhanVienChuyenMons
        public ActionResult Index(string searchString)
        {
            // 1. Lấy danh sách ban đầu, bao gồm thông tin Chuyên Môn và Nhân Viên
            var nhanVienChuyenMons = db.NhanVienChuyenMons
                .Include(n => n.ChuyenMon)
                .Include(n => n.NhanVien)
                .AsQueryable();

            // 2. Nếu có từ khóa tìm kiếm -> Thêm điều kiện lọc
            if (!String.IsNullOrEmpty(searchString))
            {
                // Tìm kiếm theo Tên Chuyên Môn
                // Bạn cũng có thể thêm || n.NhanVien.TenNV.Contains(searchString) để tìm theo tên nhân viên
                nhanVienChuyenMons = nhanVienChuyenMons.Where(n => n.ChuyenMon.TenChuyenMon.Contains(searchString));
            }

            // 3. Sắp xếp theo tên chuyên môn và trả về danh sách
            return View(nhanVienChuyenMons.OrderBy(n => n.ChuyenMon.TenChuyenMon).ToList());
        }

        // GET: NhanVienChuyenMons/Details/5
        // =============================================
        // == ĐÃ SỬA: Nhận vào 2 tham số (MaNV, MaCM) ==
        // =============================================
        public ActionResult Details(int? MaNV, int? MaCM)
        {
            if (MaNV == null || MaCM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Tìm bằng 2 khóa
            NhanVienChuyenMon nhanVienChuyenMon = db.NhanVienChuyenMons.Find(MaNV, MaCM);
            if (nhanVienChuyenMon == null)
            {
                return HttpNotFound();
            }
            return View(nhanVienChuyenMon);
        }

        // GET: NhanVienChuyenMons/Create
        public ActionResult Create()
        {
            ViewBag.MaCM = new SelectList(db.ChuyenMons, "MaCM", "TenChuyenMon");
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "TenNV");
            return View();
        }

        // POST: NhanVienChuyenMons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,MaCM")] NhanVienChuyenMon nhanVienChuyenMon)
        {
            if (ModelState.IsValid)
            {
                db.NhanVienChuyenMons.Add(nhanVienChuyenMon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaCM = new SelectList(db.ChuyenMons, "MaCM", "TenChuyenMon", nhanVienChuyenMon.MaCM);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "TenNV", nhanVienChuyenMon.MaNV);
            return View(nhanVienChuyenMon);
        }

        // GET: NhanVienChuyenMons/Edit/5
        // =============================================
        // == ĐÃ SỬA: Nhận vào 2 tham số (MaNV, MaCM) ==
        // =============================================
        public ActionResult Edit(int? MaNV, int? MaCM)
        {
            if (MaNV == null || MaCM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVienChuyenMon nhanVienChuyenMon = db.NhanVienChuyenMons.Find(MaNV, MaCM);
            if (nhanVienChuyenMon == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCM = new SelectList(db.ChuyenMons, "MaCM", "TenChuyenMon", nhanVienChuyenMon.MaCM);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "TenNV", nhanVienChuyenMon.MaNV);
            return View(nhanVienChuyenMon);
        }

        // POST: NhanVienChuyenMons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,MaCM")] NhanVienChuyenMon nhanVienChuyenMon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanVienChuyenMon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaCM = new SelectList(db.ChuyenMons, "MaCM", "TenChuyenMon", nhanVienChuyenMon.MaCM);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "TenNV", nhanVienChuyenMon.MaNV);
            return View(nhanVienChuyenMon);
        }

        // GET: NhanVienChuyenMons/Delete/5
        // =============================================
        // == ĐÃ SỬA: Nhận vào 2 tham số (MaNV, MaCM) ==
        // =============================================
        public ActionResult Delete(int? MaNV, int? MaCM)
        {
            if (MaNV == null || MaCM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVienChuyenMon nhanVienChuyenMon = db.NhanVienChuyenMons.Find(MaNV, MaCM);
            if (nhanVienChuyenMon == null)
            {
                return HttpNotFound();
            }
            return View(nhanVienChuyenMon);
        }

        // POST: NhanVienChuyenMons/Delete/5/10
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int MaNV, int MaCM)
        {
            try
            {
                var pMaNV = new System.Data.SqlClient.SqlParameter("@MaNV", MaNV);
                var pMaCM = new System.Data.SqlClient.SqlParameter("@MaCM", MaCM);

                // Gọi SP xóa an toàn
                db.Database.ExecuteSqlCommand("EXEC sp_XoaNhanVienChuyenMon @MaNV, @MaCM", pMaNV, pMaCM);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string loi = ex.Message;
                if (ex.InnerException != null) loi = ex.InnerException.InnerException.Message;
                ModelState.AddModelError("", "KHÔNG THỂ XÓA: " + loi);

                // Load lại view để hiện lỗi
                NhanVienChuyenMon nm = db.NhanVienChuyenMons.Find(MaNV, MaCM);
                return View(nm);
            }
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