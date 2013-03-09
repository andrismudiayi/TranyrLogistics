using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TranyrLogistics.Models;
using TranyrLogistics.Models.Utility;

namespace TranyrLogistics.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private TranyrLogisticsDb db = new TranyrLogisticsDb();

        //
        // GET: /Customer/

        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.CustomerGroup).Include(c => c.Country);
            return View(customers.ToList());
        }

        //
        // GET: /Customer/Details/5

        public ActionResult Details(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            customer.Country = db.Countries.Find(customer.CountryID);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // GET: /Customer/Create

        public ActionResult Create()
        {
            ViewBag.CustomerGroupID = new SelectList(db.CustomerGroups, "ID", "Name");
            ViewBag.CountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            return View();
        }

        //
        // POST: /Customer/Create

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            customer.CustomerNumber = CustomerModel.GenerateCustomerNumber(customer);
            customer.CreateDate = customer.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerGroupID = new SelectList(db.CustomerGroups, "ID", "Name", customer.CustomerGroupID);
            ViewBag.CountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            return View(customer);
        }

        //
        // GET: /Customer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerGroupID = new SelectList(db.CustomerGroups, "ID", "Name", customer.CustomerGroupID);
            ViewBag.CountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", customer.CountryID);
            return View(customer);
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            customer.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerGroupID = new SelectList(db.CustomerGroups, "ID", "Name", customer.CustomerGroupID);
            return View(customer);
        }

        //
        // GET: /Customer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            customer.Country = db.Countries.Find(customer.CountryID);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // POST: /Customer/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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