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
    public class DangKyLopsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DangKyLops
        public ActionResult Index()
        {
            var dangKyLops = db.DangKyLops.Include(d => d.KhachHang).Include(d => d.LopHoc);
            return View(dangKyLops.ToList());
        }

        // GET: DangKyLops/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyLop dangKyLop = db.DangKyLops.Find(id);
            if (dangKyLop == null)
            {
                return HttpNotFound();
            }
            return View(dangKyLop);
        }

        // GET: DangKyLops/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH");
            ViewBag.MaLop = new SelectList(db.LopHocs, "MaLop", "TenLop");
            return View();
        }

        // POST: DangKyLops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDKLop,MaKH,MaLop,NgayDangKy,TrangThai")] DangKyLop dangKyLop)
        {
            if (ModelState.IsValid)
            {
                db.DangKyLops.Add(dangKyLop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", dangKyLop.MaKH);
            ViewBag.MaLop = new SelectList(db.LopHocs, "MaLop", "TenLop", dangKyLop.MaLop);
            return View(dangKyLop);
        }

        // GET: DangKyLops/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyLop dangKyLop = db.DangKyLops.Find(id);
            if (dangKyLop == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", dangKyLop.MaKH);
            ViewBag.MaLop = new SelectList(db.LopHocs, "MaLop", "TenLop", dangKyLop.MaLop);
            return View(dangKyLop);
        }

        // POST: DangKyLops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDKLop,MaKH,MaLop,NgayDangKy,TrangThai")] DangKyLop dangKyLop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dangKyLop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", dangKyLop.MaKH);
            ViewBag.MaLop = new SelectList(db.LopHocs, "MaLop", "TenLop", dangKyLop.MaLop);
            return View(dangKyLop);
        }

        // GET: DangKyLops/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyLop dangKyLop = db.DangKyLops.Find(id);
            if (dangKyLop == null)
            {
                return HttpNotFound();
            }
            return View(dangKyLop);
        }

        // POST: DangKyLops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DangKyLop dangKyLop = db.DangKyLops.Find(id);
            db.DangKyLops.Remove(dangKyLop);
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
