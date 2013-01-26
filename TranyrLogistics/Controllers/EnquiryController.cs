using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TranyrLogistics.Models;

namespace TranyrLogistics.Controllers
{
    [Authorize]
    public class EnquiryController : Controller
    {
        private TranyrLogisticsDb db = new TranyrLogisticsDb();

        //
        // GET: /Enquiry/

        public ActionResult Index()
        {
            return View(db.Enquiries.ToList());
        }

        //
        // GET: /Enquiry/Details/5

        public ActionResult Details(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            if (enquiry == null)
            {
                return HttpNotFound();
            }
            return View(enquiry);
        }

        //
        // GET: /Enquiry/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Enquiry/Create

        [HttpPost]
        public ActionResult Create(Enquiry enquiry)
        {
            enquiry.CreateDate = enquiry.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Enquiries.Add(enquiry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(enquiry);
        }

        //
        // GET: /Enquiry/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            if (enquiry == null)
            {
                return HttpNotFound();
            }
            return View(enquiry);
        }

        //
        // POST: /Enquiry/Edit/5

        [HttpPost]
        public ActionResult Edit(Enquiry enquiry)
        {
            enquiry.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(enquiry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(enquiry);
        }

        //
        // GET: /Enquiry/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            if (enquiry == null)
            {
                return HttpNotFound();
            }
            return View(enquiry);
        }

        //
        // POST: /Enquiry/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            db.Enquiries.Remove(enquiry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}