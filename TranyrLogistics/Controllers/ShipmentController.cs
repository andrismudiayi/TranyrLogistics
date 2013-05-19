using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TranyrLogistics.Models;
using TranyrLogistics.Models.Utility;

namespace TranyrLogistics.Controllers
{
    public class ShipmentController : Controller
    {
        private TranyrLogisticsDb db = new TranyrLogisticsDb();

        //
        // GET: /Shipment/

        [Authorize(Roles = "Customer-Service, Finance, Manager, Operator")]
        public ActionResult Index(int customer_id = 0)
        {
            IQueryable<Shipment> shipments = null;
            if (customer_id > 0)
            {
                shipments = db.Shipments.Where(x => x.CustomerID == customer_id)
                    .Include(s => s.OriginCountry)
                    .Include(s => s.DestinationCountry)
                    .Include(s => s.Customer);

                ViewBag.Customer = db.Customers.Find(customer_id);
            }
            else
            {
                shipments = db.Shipments
                    .Include(s => s.OriginCountry)
                    .Include(s => s.DestinationCountry)
                    .Include(s => s.Customer);
            }
            
            return View(shipments.ToList());
        }

        //
        // GET: /Shipment/Details/5

        [Authorize(Roles = "Customer-Service, Finance, Manager, Operator")]
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

        [Authorize(Roles = "Manager, Operator")]
        public ActionResult Create(int customer_id = 0, int enquiry_id = 0)
        {
            Shipment shipment = new Shipment();
            if (customer_id > 0)
            {
                Customer customer = db.Customers.Find(customer_id);
                customer.Country = db.Countries.Find(customer.CountryID);
                shipment.CustomerID = customer.ID;
                shipment.Customer = customer;
            }

            Enquiry enquiry = null;
            if (enquiry_id > 0)
            {
                enquiry = db.Enquiries.Find(enquiry_id);
                shipment.ServiceProviderID = enquiry.PreferedServiceProviderID;
                shipment.Transport = enquiry.Transport;
                shipment.PlannedCollectionTime = enquiry.PlannedShipmentTime;
                shipment.CollectionPoint = enquiry.CollectionPoint;
                shipment.OriginCity = enquiry.OriginCity;
                shipment.OriginCountryID = enquiry.OriginCountryID;
                shipment.DestinationAddress = enquiry.DestinationAddress;
                shipment.DestinationCity = enquiry.DestinationCity;
                shipment.DestinationCountryID = enquiry.DestinationCountryID;
                shipment.Category = enquiry.Category;
                shipment.GoodsDescription = enquiry.GoodsDescription;
                shipment.NumberOfPackages = enquiry.NumberOfPackages;
                shipment.GrossWeight = enquiry.GrossWeight;
                shipment.VolumetricWeight = enquiry.VolumetricWeight;
                shipment.InsuranceRequired = enquiry.InsuranceRequired;
                shipment.ReferralEnquiryID = enquiry.ID;
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
        [Authorize(Roles = "Manager, Operator")]
        public ActionResult Create(Shipment shipment)
        {
            shipment.ReferenceNumber = ShipmentModel.GenerateReferenceNumber(shipment);
            shipment.ActualCollectionTime = shipment.PlannedCollectionTime;
            shipment.ActualTimeOfArrival = shipment.PlannedETA;
            shipment.CreateDate = shipment.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                shipment.CreatedBy = User.Identity.Name;

                if (shipment.ReferralEnquiryID > 0)
                {
                    var enquiry = db.Enquiries.Find(shipment.ReferralEnquiryID);
                    if (enquiry == null)
                    {
                        return RedirectToAction("Error");
                    }
                    enquiry.Status = Enquiry.State.CLOSED;
                    db.Entry(enquiry).State = EntityState.Modified;                    
                }

                db.Shipments.Add(shipment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return this.Create(shipment.CustomerID);
        }

        //
        // GET: /Shipment/Edit/5

        [Authorize(Roles = "Manager, Operator")]
        public ActionResult Edit(int id = 0)
        {
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            shipment.OriginCountry = db.Countries.Find(shipment.OriginCountryID);
            shipment.DestinationCountry = db.Countries.Find(shipment.DestinationCountryID);

            Customer customer = db.Customers.Find(shipment.CustomerID);
            customer.Country = db.Countries.Find(customer.CountryID);
            shipment.Customer = customer;

            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", shipment.OriginCountryID);
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", shipment.DestinationCountryID);
            ViewBag.ShippingTermsID = new SelectList(db.ShippingTerms, "ID", "Standard", shipment.ShippingTermsID);
            ViewBag.ServiceProviderID = new SelectList(db.ServiceProviders, "ID", "Name", shipment.ServiceProviderID);

            return View(shipment);
        }

        //
        // POST: /Shipment/Edit/5

        [HttpPost]
        [Authorize(Roles = "Manager, Operator")]
        public ActionResult Edit(Shipment shipment)
        {
            using (TranyrLogisticsDb db = new TranyrLogisticsDb())
            {
                Shipment currentShipment = db.Shipments.Find(shipment.ID);
                shipment.CustomerID = currentShipment.CustomerID;
                shipment.ReferenceNumber = currentShipment.ReferenceNumber;
                shipment.CreateDate = currentShipment.CreateDate;
                shipment.CreatedBy = currentShipment.CreatedBy;
            }
            shipment.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(shipment.ID);
        }

        //
        // GET: /Shipment/Delete/5

        [Authorize(Roles = "Manager")]
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
        [Authorize(Roles = "Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            Shipment shipment = db.Shipments.Find(id);
            db.Shipments.Remove(shipment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Shipment/AssignShipmentToUser

        [Authorize(Roles = "Customer-Service, Manager, Operator")]
        public ActionResult AssignShipmentToUser(int id, string username)
        {
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }

            using (TranyrMembershipDb userDb = new TranyrMembershipDb())
            {
                UserProfile userProfile = userDb.UserProfiles.FirstOrDefault(x => x.UserName == username);
                if (!Roles.IsUserInRole(userProfile.UserName, "Manager") || !Roles.IsUserInRole(userProfile.UserName, "Operator"))
                {
                    return HttpNotFound();
                }
            }

            shipment.AssignedTo = username;

            return this.Edit(shipment);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}