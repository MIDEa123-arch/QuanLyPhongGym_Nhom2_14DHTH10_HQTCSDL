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
    public class DangKyGoiTapsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DangKyGoiTaps
        public ActionResult Index()
        {
            var dangKyGoiTaps = db.DangKyGoiTaps.Include(d => d.GoiTap).Include(d => d.KhachHang);
            return View(dangKyGoiTaps.ToList());
        }

        // GET: DangKyGoiTaps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyGoiTap dangKyGoiTap = db.DangKyGoiTaps.Find(id);
            if (dangKyGoiTap == null)
            {
                return HttpNotFound();
            }
            return View(dangKyGoiTap);
        }

        // GET: DangKyGoiTaps/Create
        public ActionResult Create()
        {
            ViewBag.MaGoiTap = new SelectList(db.GoiTaps, "MaGoiTap", "TenGoi");
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH");
            return View();
        }

        // POST: DangKyGoiTaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDKGT,MaKH,MaGoiTap,NgayDangKy,NgayBatDau,NgayKetThuc,TrangThai")] DangKyGoiTap dangKyGoiTap)
        {
            if (ModelState.IsValid)
            {
                db.DangKyGoiTaps.Add(dangKyGoiTap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaGoiTap = new SelectList(db.GoiTaps, "MaGoiTap", "TenGoi", dangKyGoiTap.MaGoiTap);
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", dangKyGoiTap.MaKH);
            return View(dangKyGoiTap);
        }

        // GET: DangKyGoiTaps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyGoiTap dangKyGoiTap = db.DangKyGoiTaps.Find(id);
            if (dangKyGoiTap == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaGoiTap = new SelectList(db.GoiTaps, "MaGoiTap", "TenGoi", dangKyGoiTap.MaGoiTap);
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", dangKyGoiTap.MaKH);
            return View(dangKyGoiTap);
        }

        // POST: DangKyGoiTaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDKGT,MaKH,MaGoiTap,NgayDangKy,NgayBatDau,NgayKetThuc,TrangThai")] DangKyGoiTap dangKyGoiTap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dangKyGoiTap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaGoiTap = new SelectList(db.GoiTaps, "MaGoiTap", "TenGoi", dangKyGoiTap.MaGoiTap);
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", dangKyGoiTap.MaKH);
            return View(dangKyGoiTap);
        }

        // GET: DangKyGoiTaps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangKyGoiTap dangKyGoiTap = db.DangKyGoiTaps.Find(id);
            if (dangKyGoiTap == null)
            {
                return HttpNotFound();
            }
            return View(dangKyGoiTap);
        }

        // POST: DangKyGoiTaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DangKyGoiTap dangKyGoiTap = db.DangKyGoiTaps.Find(id);
            db.DangKyGoiTaps.Remove(dangKyGoiTap);
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
