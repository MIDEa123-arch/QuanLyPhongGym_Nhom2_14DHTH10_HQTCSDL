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
    public class HINHANHsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HINHANHs
        public ActionResult Index()
        {
            var hINHANHs = db.HINHANHs.Include(h => h.SanPham);
            return View(hINHANHs.ToList());
        }

        // GET: HINHANHs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HINHANH hINHANH = db.HINHANHs.Find(id);
            if (hINHANH == null)
            {
                return HttpNotFound();
            }
            return View(hINHANH);
        }

        // GET: HINHANHs/Create
        public ActionResult Create()
        {
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP");
            var model = new HINHANH();
            return View(model);
            
        }

        // POST: HINHANHs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHinh,MaSP,Url,IsMain")] HINHANH hINHANH)
        {
            if (ModelState.IsValid)
            {
                db.HINHANHs.Add(hINHANH);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", hINHANH.MaSP);
            return View(hINHANH);
        }
       

        // GET: HINHANHs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HINHANH hINHANH = db.HINHANHs.Find(id);
            if (hINHANH == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", hINHANH.MaSP);
            return View(hINHANH);
        }

        // POST: HINHANHs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHinh,MaSP,Url,IsMain")] HINHANH hINHANH)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hINHANH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", hINHANH.MaSP);
            return View(hINHANH);
        }

        // GET: HINHANHs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HINHANH hINHANH = db.HINHANHs.Find(id);
            if (hINHANH == null)
            {
                return HttpNotFound();
            }
            return View(hINHANH);
        }

        // POST: HINHANHs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HINHANH hINHANH = db.HINHANHs.Find(id);
            db.HINHANHs.Remove(hINHANH);
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
