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
    // Bảo vệ trang (Chỉ Manager)
    //[Authorize(Roles = "ManagerRole")]
    public class LoaiSanPhamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LoaiSanPham
        public ActionResult Index()
        {
            return View(db.LoaiSanPhams.ToList());
        }

        // GET: LoaiSanPham/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        // GET: LoaiSanPham/Create
        public ActionResult Create()
        {
            return View();
        }

        // ============================================================
        // == HÀM CREATE (POST) GỌI SP ĐỂ CHẶN TRÙNG ==
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLoaiSP,TenLoaiSP")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Chuẩn bị tham số
                    var pTenLoaiSP = new SqlParameter("@TenLoaiSP", loaiSanPham.TenLoaiSP);

                    // 2. Gọi SP Thêm Mới
                    db.Database.ExecuteSqlCommand("EXEC sp_ThemLoaiSanPham @TenLoaiSP", pTenLoaiSP);

                    // 3. Thành công -> Về Index
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // 4. Bắt lỗi trùng tên từ SQL
                    string loiNhanDuoc = ex.Message;
                    if (ex.InnerException != null && ex.InnerException.InnerException != null)
                    {
                        loiNhanDuoc = ex.InnerException.InnerException.Message;
                    }
                    // Hiển thị lỗi đỏ trên giao diện
                    ModelState.AddModelError("", "KHÔNG THÀNH CÔNG: " + loiNhanDuoc);
                }
            }

            return View(loaiSanPham);
        }

        // GET: LoaiSanPham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        // ============================================================
        // == HÀM EDIT (POST) GỌI SP ĐỂ CHẶN TRÙNG ==
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoaiSP,TenLoaiSP")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Chuẩn bị tham số
                    var pMaLoaiSP = new SqlParameter("@MaLoaiSP", loaiSanPham.MaLoaiSP);
                    var pTenLoaiSP = new SqlParameter("@TenLoaiSP", loaiSanPham.TenLoaiSP);

                    // 2. Gọi SP Sửa
                    db.Database.ExecuteSqlCommand("EXEC sp_SuaLoaiSanPham @MaLoaiSP, @TenLoaiSP",
                        pMaLoaiSP, pTenLoaiSP);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // 4. Bắt lỗi trùng tên từ SQL
                    string loiNhanDuoc = ex.Message;
                    if (ex.InnerException != null && ex.InnerException.InnerException != null)
                    {
                        loiNhanDuoc = ex.InnerException.InnerException.Message;
                    }
                    ModelState.AddModelError("", "KHÔNG THÀNH CÔNG: " + loiNhanDuoc);
                }
            }
            return View(loaiSanPham);
        }

        // GET: LoaiSanPham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        // POST: LoaiSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoaiSanPham loaiSanPham = db.LoaiSanPhams.Find(id);
            // Lưu ý: Nếu Loại SP này đang được dùng bởi Sản Phẩm nào đó, 
            // việc xóa sẽ bị lỗi ràng buộc khóa ngoại (Foreign Key Constraint).
            // Bạn có thể cần thêm try-catch ở đây nếu muốn xử lý êm đẹp.
            db.LoaiSanPhams.Remove(loaiSanPham);
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