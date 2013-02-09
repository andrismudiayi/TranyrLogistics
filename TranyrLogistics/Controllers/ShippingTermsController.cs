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

        public ActionResult Index()
        {
            return View(db.ShippingTerms.ToList());
        }

        //
        // GET: /ShipmentTerms/Details/5

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

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ShipmentTerms/Create

        [HttpPost]
        public ActionResult Create(ShippingTerms shipmentterms)
        {
            if (ModelState.IsValid)
            {
                db.ShippingTerms.Add(shipmentterms);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shipmentterms);
        }

        //
        // GET: /ShipmentTerms/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ShippingTerms shipmentterms = db.ShippingTerms.Find(id);
            if (shipmentterms == null)
            {
                return HttpNotFound();
            }
            return View(shipmentterms);
        }

        //
        // POST: /ShipmentTerms/Edit/5

        [HttpPost]
        public ActionResult Edit(ShippingTerms shipmentterms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipmentterms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shipmentterms);
        }

        //
        // GET: /ShipmentTerms/Delete/5

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