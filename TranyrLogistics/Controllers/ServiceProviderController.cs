using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TranyrLogistics.Models;

namespace TranyrLogistics.Controllers
{
    [Authorize]
    public class ServiceProviderController : Controller
    {
        private TranyrLogisticsDb db = new TranyrLogisticsDb();

        //
        // GET: /ServiceProvider/

        public ActionResult Index()
        {
            IQueryable<ServiceProvider> serviceProviders = db.ServiceProviders.Include(s => s.Country);
            return View(serviceProviders.ToList().OrderBy(x => x.Name));
        }

        //
        // GET: /ServiceProvider/Details/5

        public ActionResult Details(int id = 0)
        {
            ServiceProvider serviceprovider = db.ServiceProviders.Find(id);
            serviceprovider.Country = db.Countries.Find(serviceprovider.CountryID);
            if (serviceprovider == null)
            {
                return HttpNotFound();
            }
            return View(serviceprovider);
        }

        //
        // GET: /ServiceProvider/Create

        public ActionResult Create()
        {
            ViewBag.ServiceProviderGroupID = new SelectList(db.ServiceProviderGroups, "ID", "Name");
            ViewBag.CountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            return View();
        }

        //
        // POST: /ServiceProvider/Create

        [HttpPost]
        public ActionResult Create(ServiceProvider serviceprovider)
        {
            serviceprovider.CreateDate = serviceprovider.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.ServiceProviders.Add(serviceprovider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serviceprovider);
        }

        //
        // GET: /ServiceProvider/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ServiceProvider serviceprovider = db.ServiceProviders.Find(id);
            if (serviceprovider == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceProviderGroupID = new SelectList(db.ServiceProviderGroups, "ID", "Name", serviceprovider.ServiceProviderGroupID);
            ViewBag.CountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", serviceprovider.CountryID);
            return View(serviceprovider);
        }

        //
        // POST: /ServiceProvider/Edit/5

        [HttpPost]
        public ActionResult Edit(ServiceProvider serviceprovider)
        {
            serviceprovider.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(serviceprovider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serviceprovider);
        }

        //
        // GET: /ServiceProvider/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ServiceProvider serviceprovider = db.ServiceProviders.Find(id);
            serviceprovider.Country = db.Countries.Find(serviceprovider.CountryID);
            if (serviceprovider == null)
            {
                return HttpNotFound();
            }
            return View(serviceprovider);
        }

        //
        // POST: /ServiceProvider/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceProvider serviceprovider = db.ServiceProviders.Find(id);
            db.ServiceProviders.Remove(serviceprovider);
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