using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TranyrLogistics.Models;

namespace TranyrLogistics.Controllers
{
    public class ServiceProviderController : Controller
    {
        private TranyrLogisticsDb db = new TranyrLogisticsDb();

        //
        // GET: /ServiceProvider/

        [Authorize(Roles = "Administrator, Finance, Manager")]
        public ActionResult Index()
        {
            IQueryable<ServiceProvider> serviceProviders = db.ServiceProviders.Include(s => s.Country);
            return View(serviceProviders.ToList().OrderBy(x => x.Name));
        }

        //
        // GET: /ServiceProvider/Details/5

        [Authorize(Roles = "Administrator, Finance, Manager")]
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

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.ServiceProviderGroupID = new SelectList(db.ServiceProviderGroups, "ID", "Name");
            return View();
        }

        //
        // POST: /ServiceProvider/Create

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(ServiceProvider serviceprovider)
        {
            serviceprovider.CreateDate = serviceprovider.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.ServiceProviders.Add(serviceprovider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.ServiceProviderGroupID = new SelectList(db.ServiceProviderGroups, "ID", "Name");

            return View(serviceprovider);
        }

        //
        // GET: /ServiceProvider/Edit/5

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id = 0)
        {
            ServiceProvider serviceProvider = db.ServiceProviders.Find(id);
            if (serviceProvider == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", serviceProvider.CountryID);
            ViewBag.ServiceProviderGroupID = new SelectList(db.ServiceProviderGroups, "ID", "Name", serviceProvider.ServiceProviderGroupID);

            return View(serviceProvider);
        }

        //
        // POST: /ServiceProvider/Edit/5

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(ServiceProvider serviceProvider)
        {
            using (TranyrLogisticsDb db = new TranyrLogisticsDb())
            {
                ServiceProvider currentServiceprovider = db.ServiceProviders.Find(serviceProvider.ID);
                serviceProvider.CreateDate = currentServiceprovider.CreateDate;
            }
            serviceProvider.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(serviceProvider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", serviceProvider.CountryID);
            ViewBag.ServiceProviderGroupID = new SelectList(db.ServiceProviderGroups, "ID", "Name", serviceProvider.ServiceProviderGroupID);

            return View(serviceProvider);
        }

        //
        // GET: /ServiceProvider/Delete/5

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id = 0)
        {
            ServiceProvider serviceProvider = db.ServiceProviders.Find(id);
            serviceProvider.Country = db.Countries.Find(serviceProvider.CountryID);
            if (serviceProvider == null)
            {
                return HttpNotFound();
            }
            return View(serviceProvider);
        }

        //
        // POST: /ServiceProvider/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator, Manager")]
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