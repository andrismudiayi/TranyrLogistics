using System.Data;
using System.Linq;
using System.Web.Mvc;
using TranyrLogistics.Models;

namespace TranyrLogistics.Controllers
{
    public class ShippingTermsController : Controller
    {
        private TranyrLogisticsDb db = new TranyrLogisticsDb();

        //
        // GET: /ShipmentTerms/

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            return View(db.ShippingTerms.ToList());
        }

        //
        // GET: /ShipmentTerms/Details/5

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Details(int id = 0)
        {
            ShippingTerms shipmentterms = db.ShippingTerms.Find(id);
            if (shipmentterms == null)
            {
                return HttpNotFound();
            }
            return View(shipmentterms);
        }

        //
        // GET: /ShipmentTerms/Create

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ShipmentTerms/Create

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(ShippingTerms shipmentTerms)
        {
            if (ModelState.IsValid)
            {
                db.ShippingTerms.Add(shipmentTerms);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shipmentTerms);
        }

        //
        // GET: /ShipmentTerms/Edit/5

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id = 0)
        {
            ShippingTerms shipmentTerms = db.ShippingTerms.Find(id);
            if (shipmentTerms == null)
            {
                return HttpNotFound();
            }
            return View(shipmentTerms);
        }

        //
        // POST: /ShipmentTerms/Edit/5

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(ShippingTerms shipmentTerms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipmentTerms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shipmentTerms);
        }

        //
        // GET: /ShipmentTerms/Delete/5

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id = 0)
        {
            ShippingTerms shipmentterms = db.ShippingTerms.Find(id);
            if (shipmentterms == null)
            {
                return HttpNotFound();
            }
            return View(shipmentterms);
        }

        //
        // POST: /ShipmentTerms/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            ShippingTerms shipmentterms = db.ShippingTerms.Find(id);
            db.ShippingTerms.Remove(shipmentterms);
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