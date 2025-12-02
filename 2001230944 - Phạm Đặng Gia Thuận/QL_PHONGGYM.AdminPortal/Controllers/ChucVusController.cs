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
    public class ChucVusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChucVus
        public ActionResult Index()
        {
            return View(db.ChucVus.ToList());
        }

        // GET: ChucVus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChucVu chucVu = db.ChucVus.Find(id);
            if (chucVu == null)
            {
                return HttpNotFound();
            }
            return View(chucVu);
        }

        // GET: ChucVus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChucVus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaChucVu,TenChucVu")] ChucVu chucVu)
        {
            if (ModelState.IsValid)
            {
                db.ChucVus.Add(chucVu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chucVu);
        }

        // GET: ChucVus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChucVu chucVu = db.ChucVus.Find(id);
            if (chucVu == null)
            {
                return HttpNotFound();
            }
            return View(chucVu);
        }

        // POST: ChucVus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaChucVu,TenChucVu")] ChucVu chucVu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chucVu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chucVu);
        }

        // GET: ChucVus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChucVu chucVu = db.ChucVus.Find(id);
            if (chucVu == null)
            {
                return HttpNotFound();
            }
            return View(chucVu);
        }

        // POST: ChucVus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // Gọi Stored Procedure "sp_XoaChucVu"
                var pMaChucVu = new System.Data.SqlClient.SqlParameter("@MaChucVu", id);
                db.Database.ExecuteSqlCommand("EXEC sp_XoaChucVu @MaChucVu", pMaChucVu);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Bắt lỗi từ SQL (Ví dụ: Cố xóa chức vụ "PT")
                string loi = ex.Message;
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    loi = ex.InnerException.InnerException.Message;
                }

                // Hiển thị lỗi đỏ
                ModelState.AddModelError("", "KHÔNG THỂ XÓA: " + loi);

                // Trả về trang để xem lỗi
                ChucVu chucVu = db.ChucVus.Find(id);
                return View(chucVu);
            }
        }
    }
}
