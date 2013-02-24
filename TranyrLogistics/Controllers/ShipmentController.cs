using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TranyrLogistics.Models;
using TranyrLogistics.Models.Utility;

namespace TranyrLogistics.Controllers
{
    public class ShipmentController : Controller
    {
        private TranyrLogisticsDb db = new TranyrLogisticsDb();

        //
        // GET: /Shipment/

        public ActionResult Index(int customer_id = 0)
        {
            IQueryable<Shipment> shipments = db.Shipments.Include(s => s.OriginCountry).Include(s => s.DestinationCountry);
            if (customer_id > 0)
            {
                shipments.Where(x => x.CustomerID == customer_id);
                ViewBag.Customer = db.Customers.FirstOrDefault(x => x.ID == customer_id);
            }
            else
            {
                shipments.Include(s => s.Customer);
            }
            
            return View(shipments.ToList());
        }

        //
        // GET: /Shipment/Details/5

        public ActionResult Details(int id = 0)
        {
            Shipment shipment = db.Shipments.Find(id);
            shipment.OriginCountry = db.Countries.Find(shipment.OriginCountryID);
            shipment.DestinationCountry = db.Countries.Find(shipment.DestinationCountryID);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        //
        // GET: /Shipment/Create

        public ActionResult Create(int customer_id = 0)
        {
            Shipment shipment = new Shipment();
            if (customer_id > 0)
            {
                Customer customer = db.Customers.Find(customer_id);
                customer.Country = db.Countries.Find(customer.CountryID);
                shipment.CustomerID = customer.ID;
                shipment.Customer = customer;
            }
            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", shipment.OriginCountryID);
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", shipment.DestinationCountryID);
            ViewBag.ShippingTermsID = new SelectList(db.ShippingTerms, "ID", "Standard", shipment.ShippingTermsID);
            ViewBag.ServiceProviderID = new SelectList(db.ServiceProviders, "ID", "Name", shipment.ServiceProviderID);
            return View(shipment);
        }

        //
        // POST: /Shipment/Create

        [HttpPost]
        public ActionResult Create(Shipment shipment)
        {
            shipment.ReferenceNumber = ShipmentModel.GenerateReferenceNumber(shipment);
            shipment.CollectionDate = shipment.PlannedCollectionDate;
            shipment.ActualTimeOfArrival = shipment.PlannedETA;
            shipment.ActualPickUpTime = shipment.PlannedPickUpTime;
            shipment.CreateDate = shipment.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Shipments.Add(shipment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return this.Create(shipment.CustomerID);
        }

        //
        // GET: /Shipment/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Shipment shipment = db.Shipments.Find(id);
            shipment.OriginCountry = db.Countries.Find(shipment.OriginCountryID);
            shipment.DestinationCountry = db.Countries.Find(shipment.DestinationCountryID);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", shipment.OriginCountryID);
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", shipment.DestinationCountryID);
            ViewBag.ShippingTermsID = new SelectList(db.ShippingTerms, "ID", "Standard", shipment.ShippingTermsID);
            ViewBag.ServiceProviderID = new SelectList(db.ServiceProviders, "ID", "Name", shipment.ServiceProviderID);
            return View(shipment);
        }

        //
        // POST: /Shipment/Edit/5

        [HttpPost]
        public ActionResult Edit(Shipment shipment)
        {
            shipment.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(shipment.ShipmentID);
        }

        //
        // GET: /Shipment/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Shipment shipment = db.Shipments.Find(id);
            shipment.OriginCountry = db.Countries.Find(shipment.OriginCountryID);
            shipment.DestinationCountry = db.Countries.Find(shipment.DestinationCountryID);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        //
        // POST: /Shipment/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Shipment shipment = db.Shipments.Find(id);
            db.Shipments.Remove(shipment);
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