using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TranyrLogistics.Controllers.Utility;
using TranyrLogistics.Models;

namespace TranyrLogistics.Controllers
{
    [Authorize]
    public class ShipmentDocumentController : Controller
    {

        private TranyrLogisticsDb db = new TranyrLogisticsDb();

        //
        // GET: /ShipmentDocumentation/

        public ActionResult Index(string customer_number = "", int shipment_id = 0)
        {
            if (customer_number == string.Empty || shipment_id == 0)
            {
                return RedirectToAction("Error");
            }
            Customer customer = db.Customers.FirstOrDefault(x => x.CustomerNumber == customer_number);
            ViewBag.Customer = customer;

            Shipment shipment = db.Shipments.FirstOrDefault(x => x.ShipmentID == shipment_id);
            ViewBag.Shipment = shipment;

            return View(db.ShipmentDocuments.Where(x => x.CustomerNumber == customer_number && x.ShipmentID == shipment_id).ToList());
        }

        //
        // GET: /ShipmentDocumentation/Details/5

        public ActionResult Details(int id = 0)
        {
            ShipmentDocument shipmentdocumentation = db.ShipmentDocuments.Find(id);
            if (shipmentdocumentation == null)
            {
                return HttpNotFound();
            }
            return View(shipmentdocumentation);
        }

        //
        // GET: /ShipmentDocumentation/Create

        public ActionResult Create(string customer_number = "", int shipment_id = 0)
        {
            if (customer_number == string.Empty || shipment_id == 0)
            {
                return RedirectToAction("Error");
            }
            ShipmentDocument shipmentDocument = new ShipmentDocument();
            shipmentDocument.CustomerNumber = customer_number;
            shipmentDocument.ShipmentID = shipment_id;
            return View(shipmentDocument);
        }

        //
        // POST: /ShipmentDocumentation/Create

        [HttpPost]
        public ActionResult Create(ShipmentDocument shipmentDocument)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files["FilePath"];
                var fileName = Path.GetFileName(file.FileName);
                string uploadDirectoryPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory +
                    @"Uploads\ShipmentDocs\" + shipmentDocument.CustomerNumber + @"\"
                );
                string savedFileName = Path.Combine(
                     uploadDirectoryPath,
                     DateTime.Now.Ticks + Path.GetExtension(fileName)
                 );
                if (!Directory.Exists(uploadDirectoryPath))
                {
                    Directory.CreateDirectory(uploadDirectoryPath);
                }
                file.SaveAs(savedFileName);
                shipmentDocument.ActualFileName = fileName;
                shipmentDocument.FilePathOnDisc = savedFileName;
            }

            shipmentDocument.CreateDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.ShipmentDocuments.Add(shipmentDocument);
                db.SaveChanges();

                return RedirectToAction("Index", new { customer_number = shipmentDocument.CustomerNumber, shipment_id=shipmentDocument.ShipmentID });
            }

            return View(shipmentDocument);
        }

        //
        // GET: /ShipmentDocumentation/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ShipmentDocument shipmentDocument = db.ShipmentDocuments.Find(id);
            if (shipmentDocument == null)
            {
                return HttpNotFound();
            }
            return View(shipmentDocument);
        }

        //
        // POST: /ShipmentDocumentation/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ShipmentDocument shipmentDocument = db.ShipmentDocuments.Find(id);
            db.ShipmentDocuments.Remove(shipmentDocument);
            db.SaveChanges();

            System.IO.File.Delete(shipmentDocument.FilePathOnDisc);
            if (Directory.GetFiles(Path.GetDirectoryName(shipmentDocument.FilePathOnDisc)).Count() == 0)
            {
                Directory.Delete(Path.GetDirectoryName(shipmentDocument.FilePathOnDisc));
            }

            return RedirectToAction("Index", new { customer_number = shipmentDocument.CustomerNumber, shipment_id = shipmentDocument.ShipmentID });
        }

        //
        // GET: /ShipmentDocumentation/DownloadFile/5

        public ActionResult DownloadFile(int id = 0)
        {
            ShipmentDocument shipmentDocument = db.ShipmentDocuments.Find(id);

            return File(shipmentDocument.FilePathOnDisc,
                FileContent.GetType(Path.GetExtension(shipmentDocument.ActualFileName)),
                string.Format("{0}", shipmentDocument.ActualFileName)
            );
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}