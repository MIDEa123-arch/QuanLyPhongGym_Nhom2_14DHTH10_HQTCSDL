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
    public class DiaChisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DiaChis
        public ActionResult Index()
        {
            var diaChis = db.DiaChis.Include(d => d.KhachHang);
            return View(diaChis.ToList());
        }

        // GET: DiaChis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaChi diaChi = db.DiaChis.Find(id);
            if (diaChi == null)
            {
                return HttpNotFound();
            }
            return View(diaChi);
        }

        // GET: DiaChis/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH");
            return View();
        }

        // POST: DiaChis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDC,MaKH,TinhThanhPho,QuanHuyen,PhuongXa,DiaChiCuThe,LaDiaChiMacDinh")] DiaChi diaChi)
        {
            if (ModelState.IsValid)
            {
                db.DiaChis.Add(diaChi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", diaChi.MaKH);
            return View(diaChi);
        }

        // GET: DiaChis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaChi diaChi = db.DiaChis.Find(id);
            if (diaChi == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", diaChi.MaKH);
            return View(diaChi);
        }

        // POST: DiaChis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDC,MaKH,TinhThanhPho,QuanHuyen,PhuongXa,DiaChiCuThe,LaDiaChiMacDinh")] DiaChi diaChi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diaChi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "TenKH", diaChi.MaKH);
            return View(diaChi);
        }

        // GET: DiaChis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaChi diaChi = db.DiaChis.Find(id);
            if (diaChi == null)
            {
                return HttpNotFound();
            }
            return View(diaChi);
        }

        // POST: DiaChis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiaChi diaChi = db.DiaChis.Find(id);
            db.DiaChis.Remove(diaChi);
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
