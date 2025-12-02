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
    public class KhachHangsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: KhachHangs
        public ActionResult Index()
        {
            return View(db.KhachHangs.ToList());
        }
        public ActionResult Index1()
        {
            return View(db.KhachHangs.ToList());
        }

        // GET: KhachHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KhachHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,TenKH,NgaySinh,SDT,Email,MaLoaiKH,GioiTinh,TenDangNhap,MatKhau")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khachHang);
        }

        // GET: KhachHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,TenKH,NgaySinh,SDT,Email,MaLoaiKH,GioiTinh,TenDangNhap,MatKhau")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // Gọi Stored Procedure "sp_XoaKhachHang"
                var pMaKH = new System.Data.SqlClient.SqlParameter("@MaKH", id);
                db.Database.ExecuteSqlCommand("EXEC sp_XoaKhachHang @MaKH", pMaKH);

                // Thành công -> Quay về danh sách
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Bắt lỗi từ SQL (Ví dụ: Khách có Hóa đơn không xóa được)
                string loi = ex.Message;
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    loi = ex.InnerException.InnerException.Message;
                }

                // Hiển thị lỗi đỏ cho người dùng
                ModelState.AddModelError("", "KHÔNG THỂ XÓA: " + loi);

                // Trả về trang xác nhận xóa để người dùng đọc lỗi
                // (Phải load lại dữ liệu để hiển thị)
                KhachHang khachHang = db.KhachHangs
                    .Include(k => k.LoaiKhachHang)
                    .SingleOrDefault(k => k.MaKH == id);

                return View(khachHang);
            }
        }

    }
}
