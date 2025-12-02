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
    // Bảo vệ toàn bộ trang này (chỉ Manager mới được vào)
   // [Authorize(Roles = "ManagerRole")]
    public class DangKyPTsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DangKyPTs
        public ActionResult Index()
        {
            var dangKyPTs = db.DangKyPTs.Include(d => d.KhachHang).Include(d => d.NhanVien.ChucVu); // Thêm ChucVu
            return View(dangKyPTs.ToList());
        }

        // GET: DangKyPTs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Thêm Include
            DangKyPT dangKyPT = db.DangKyPTs
                .Include(d => d.KhachHang)
                .Include(d => d.NhanVien.ChucVu)
                .SingleOrDefault(d => d.MaDKPT == id);

            if (dangKyPT == null)
            {
                return HttpNotFound();
            }
            return View(dangKyPT);
        }

        // GET: DangKyPTs/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH");

            // =============================================
            // == 1. ĐÃ SỬA: "Xổ ra hết" TẤT CẢ nhân viên ==
            // =============================================
            var tatCaNhanVien = db.NhanViens
                .Include(n => n.ChucVu) // Nối bảng ChucVu
                .ToList() // Lấy về
                .Select(nv => new SelectListItem
                {
                    Value = nv.MaNV.ToString(),
                    // Hiển thị tên kèm chức vụ: "Trần Thị Yến (HLV Lớp)"
                    Text = $"{nv.TenNV} ({nv.ChucVu?.TenChucVu ?? "Chưa có CV"})"
                });

            ViewBag.MaNV = new SelectList(tatCaNhanVien, "Value", "Text");
            return View();
        }

        // POST: DangKyPTs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDKPT,MaKH,MaNV,SoBuoi,GiaMoiBuoi,NgayDangKy,TrangThai")] DangKyPT dangKyPT)
        {
            // =============================================
            // == 2. Hàng rào an toàn (GIỮ NGUYÊN) ==
            // (Nó sẽ "chặn" nếu bạn chọn HLV Lớp)
            // =============================================
            var selectedNhanVien = db.NhanViens
                .Include(n => n.ChucVu)
                .SingleOrDefault(n => n.MaNV == dangKyPT.MaNV);

            bool isPT = selectedNhanVien != null && selectedNhanVien.ChucVu != null && selectedNhanVien.ChucVu.TenChucVu == "PT";

            if (!isPT)
            {
                // Nếu người được chọn không phải PT -> Thêm lỗi vào Model
                ModelState.AddModelError("MaNV", "Nhân viên được chọn không phải là PT (Huấn luyện viên cá nhân).");
            }
            // =============================================

            if (ModelState.IsValid)
            {
                dangKyPT.NgayDangKy = DateTime.Today;
                dangKyPT.TrangThai = "Chờ duyệt"; // Giống SP của bạn

                db.DangKyPTs.Add(dangKyPT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu thất bại, tải lại Dropdown (đã "xổ ra hết")
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", dangKyPT.MaKH);
            var tatCaNhanVien = db.NhanViens.Include(n => n.ChucVu).ToList().Select(nv => new SelectListItem
            {
                Value = nv.MaNV.ToString(),
                Text = $"{nv.TenNV} ({nv.ChucVu?.TenChucVu ?? "Chưa có CV"})"
            });
            ViewBag.MaNV = new SelectList(tatCaNhanVien, "Value", "Text", dangKyPT.MaNV);
            return View(dangKyPT);
        }

        // GET: DangKyPTs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyPT dangKyPT = db.DangKyPTs.Find(id);
            if (dangKyPT == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", dangKyPT.MaKH);

            // =============================================
            // == 3. ĐÃ SỬA: "Xổ ra hết" TẤT CẢ nhân viên ==
            // =============================================
            var tatCaNhanVien = db.NhanViens.Include(n => n.ChucVu).ToList().Select(nv => new SelectListItem
            {
                Value = nv.MaNV.ToString(),
                Text = $"{nv.TenNV} ({nv.ChucVu?.TenChucVu ?? "Chưa có CV"})"
            });
            ViewBag.MaNV = new SelectList(tatCaNhanVien, "Value", "Text", dangKyPT.MaNV);
            return View(dangKyPT);
        }

        // POST: DangKyPTs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDKPT,MaKH,MaNV,SoBuoi,GiaMoiBuoi,NgayDangKy,TrangThai")] DangKyPT dangKyPT)
        {
            // =============================================
            // == 4. Hàng rào an toàn (GIỮ NGUYÊN) ==
            // =============================================
            var selectedNhanVien = db.NhanViens
                .Include(n => n.ChucVu)
                .SingleOrDefault(n => n.MaNV == dangKyPT.MaNV);

            bool isPT = selectedNhanVien != null && selectedNhanVien.ChucVu != null && selectedNhanVien.ChucVu.TenChucVu == "PT";

            if (!isPT)
            {
                ModelState.AddModelError("MaNV", "Nhân viên được chọn không phải là PT (Huấn luyện viên cá nhân).");
            }
            // =============================================

            if (ModelState.IsValid)
            {
                db.Entry(dangKyPT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", dangKyPT.MaKH);
            var tatCaNhanVien = db.NhanViens.Include(n => n.ChucVu).ToList().Select(nv => new SelectListItem
            {
                Value = nv.MaNV.ToString(),
                Text = $"{nv.TenNV} ({nv.ChucVu?.TenChucVu ?? "Chưa có CV"})"
            });
            ViewBag.MaNV = new SelectList(tatCaNhanVien, "Value", "Text", dangKyPT.MaNV);
            return View(dangKyPT);
        }

        // GET: DangKyPTs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Thêm Include
            DangKyPT dangKyPT = db.DangKyPTs
                .Include(d => d.KhachHang)
                .Include(d => d.NhanVien.ChucVu)
                .SingleOrDefault(d => d.MaDKPT == id);

            if (dangKyPT == null)
            {
                return HttpNotFound();
            }
            return View(dangKyPT);
        }

        // POST: DangKyPTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DangKyPT dangKyPT = db.DangKyPTs.Find(id);
            db.DangKyPTs.Remove(dangKyPT);
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