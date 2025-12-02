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
using System.Data.SqlClient; // Cần thêm để dùng SqlParameter

namespace QL_PHONGGYM.AdminPortal.Controllers
{
    //[Authorize(Roles = "ManagerRole")]
    public class ChuyenMonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChuyenMons
        public ActionResult Index()
        {
            // Include NhanVienChuyenMons để hiển thị danh sách nhân viên dạy môn này (như yêu cầu trước)
            var chuyenMons = db.ChuyenMons
                .Include(c => c.NhanVienChuyenMons.Select(n => n.NhanVien))
                .Include(c => c.NhanVienChuyenMons.Select(n => n.NhanVien.ChucVu));
            return View(chuyenMons.ToList());
        }

        // GET: ChuyenMons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenMon chuyenMon = db.ChuyenMons.Find(id);
            if (chuyenMon == null)
            {
                return HttpNotFound();
            }
            return View(chuyenMon);
        }

        // GET: ChuyenMons/Create
        public ActionResult Create()
        {
            return View();
        }

        // ============================================================
        // == HÀM CREATE (POST) GỌI STORED PROCEDURE ĐỂ CHẶN TRÙNG ==
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaCM,TenChuyenMon,MoTa")] ChuyenMon chuyenMon)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var pTenChuyenMon = new SqlParameter("@TenChuyenMon", chuyenMon.TenChuyenMon);
                    // Xử lý MoTa nếu null
                    var pMoTa = new SqlParameter("@MoTa", (object)chuyenMon.MoTa ?? DBNull.Value);

                    // Gọi SP Thêm Mới
                    db.Database.ExecuteSqlCommand("EXEC sp_ThemChuyenMon @TenChuyenMon, @MoTa",
                        pTenChuyenMon, pMoTa);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Bắt lỗi trùng tên từ SQL
                    string loiNhanDuoc = ex.Message;
                    if (ex.InnerException != null && ex.InnerException.InnerException != null)
                    {
                        loiNhanDuoc = ex.InnerException.InnerException.Message;
                    }
                    ModelState.AddModelError("", "KHÔNG THÀNH CÔNG: " + loiNhanDuoc);
                }
            }

            return View(chuyenMon);
        }

        // GET: ChuyenMons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenMon chuyenMon = db.ChuyenMons.Find(id);
            if (chuyenMon == null)
            {
                return HttpNotFound();
            }
            return View(chuyenMon);
        }

        // ============================================================
        // == HÀM EDIT (POST) GỌI STORED PROCEDURE ĐỂ CHẶN TRÙNG ==
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaCM,TenChuyenMon,MoTa")] ChuyenMon chuyenMon)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var pMaCM = new SqlParameter("@MaCM", chuyenMon.MaCM);
                    var pTenChuyenMon = new SqlParameter("@TenChuyenMon", chuyenMon.TenChuyenMon);
                    var pMoTa = new SqlParameter("@MoTa", (object)chuyenMon.MoTa ?? DBNull.Value);

                    // Gọi SP Sửa
                    db.Database.ExecuteSqlCommand("EXEC sp_SuaChuyenMon @MaCM, @TenChuyenMon, @MoTa",
                        pMaCM, pTenChuyenMon, pMoTa);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Bắt lỗi trùng tên từ SQL
                    string loiNhanDuoc = ex.Message;
                    if (ex.InnerException != null && ex.InnerException.InnerException != null)
                    {
                        loiNhanDuoc = ex.InnerException.InnerException.Message;
                    }
                    ModelState.AddModelError("", "KHÔNG THÀNH CÔNG: " + loiNhanDuoc);
                }
            }
            return View(chuyenMon);
        }

        // GET: ChuyenMons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenMon chuyenMon = db.ChuyenMons.Find(id);
            if (chuyenMon == null)
            {
                return HttpNotFound();
            }

            // ============================================================
            // == TÍCH HỢP FUNCTION SQL MỚI ==
            // ============================================================
            int soLuongNV = db.Database.SqlQuery<int>("SELECT dbo.f_DemSoLuongNhanVienTheoChuyenMon(@p0)", id).SingleOrDefault();
            ViewBag.SoLuongNhanVienDangCo = soLuongNV;
            // ============================================================

            return View(chuyenMon);
        }

        // POST: ChuyenMons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var pMaCM = new System.Data.SqlClient.SqlParameter("@MaCM", id);

                // Gọi SP xóa an toàn
                db.Database.ExecuteSqlCommand("EXEC sp_XoaChuyenMon @MaCM", pMaCM);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string loi = ex.Message;
                if (ex.InnerException != null) loi = ex.InnerException.InnerException.Message;
                ModelState.AddModelError("", "KHÔNG THỂ XÓA: " + loi);

                ChuyenMon cm = db.ChuyenMons.Find(id);
                return View(cm);
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