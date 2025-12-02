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
    public class GoiTapsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GoiTaps
        public ActionResult Index()
        {
            return View(db.GoiTaps.ToList());
        }

        // GET: GoiTaps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoiTap goiTap = db.GoiTaps.Find(id);
            if (goiTap == null)
            {
                return HttpNotFound();
            }
            return View(goiTap);
        }

        // GET: GoiTaps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GoiTaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaGoiTap,TenGoi,ThoiHan,Gia,MoTa")] GoiTap goiTap)
        {
            if (ModelState.IsValid)
            {
                db.GoiTaps.Add(goiTap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goiTap);
        }

        // GET: GoiTaps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoiTap goiTap = db.GoiTaps.Find(id);
            if (goiTap == null)
            {
                return HttpNotFound();
            }
            return View(goiTap);
        }

        // POST: GoiTaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaGoiTap,TenGoi,ThoiHan,Gia,MoTa")] GoiTap goiTap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goiTap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goiTap);
        }

        // GET: GoiTaps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoiTap goiTap = db.GoiTaps.Find(id);
            if (goiTap == null)
            {
                return HttpNotFound();
            }
            return View(goiTap);
        }

        // POST: GoiTaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GoiTap goiTap = db.GoiTaps.Find(id);
            db.GoiTaps.Remove(goiTap);
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
