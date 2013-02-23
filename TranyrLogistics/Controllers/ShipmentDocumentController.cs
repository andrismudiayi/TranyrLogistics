using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TranyrLogistics.Models;

namespace TranyrLogistics.Controllers
{
    public class ShipmentDocumentController : Controller
    {
        private Dictionary<string, string> contentTypeConfig = null;

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
                 this.getContentType(Path.GetExtension(shipmentDocument.ActualFileName)),
                  string.Format("{0}", shipmentDocument.ActualFileName));
        }

        protected Dictionary<string, string> ContentTypeConfig
        {
            get
            {
                if (this.contentTypeConfig == null)
                {
                    this.contentTypeConfig = new Dictionary<string, string>();
                    // Images
                    this.contentTypeConfig.Add(".bmp", "image/bmp");
                    this.contentTypeConfig.Add(".gif", "image/gif");
                    this.contentTypeConfig.Add(".jpeg", "image/jpeg");
                    this.contentTypeConfig.Add(".jpg", "image/jpeg");
                    this.contentTypeConfig.Add(".png", "image/png");
                    this.contentTypeConfig.Add(".tif", "image/tiff");
                    this.contentTypeConfig.Add(".tiff", "image/tiff");
                    // Documents
                    this.contentTypeConfig.Add(".doc", "application/msword");
                    this.contentTypeConfig.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                    this.contentTypeConfig.Add(".pdf", "application/pdf");
                    // Slideshows
                    this.contentTypeConfig.Add(".ppt", "application/vnd.ms-powerpoint");
                    this.contentTypeConfig.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
                    // Data
                    this.contentTypeConfig.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    this.contentTypeConfig.Add(".xls", "application/vnd.ms-excel");
                    this.contentTypeConfig.Add(".csv", "text/csv");
                    this.contentTypeConfig.Add(".xml", "text/xml");
                    this.contentTypeConfig.Add(".txt", "text/plain");
                    // Compressed Folders
                    this.contentTypeConfig.Add(".zip", "application/zip");
                    // Audio
                    this.contentTypeConfig.Add(".ogg", "application/ogg");
                    this.contentTypeConfig.Add(".mp3", "audio/mpeg");
                    this.contentTypeConfig.Add(".wma", "audio/x-ms-wma");
                    this.contentTypeConfig.Add(".wav", "audio/x-wav");
                    // Video
                    this.contentTypeConfig.Add(".wmv", "audio/x-ms-wmv");
                    this.contentTypeConfig.Add(".swf", "application/x-shockwave-flash");
                    this.contentTypeConfig.Add(".avi", "video/avi");
                    this.contentTypeConfig.Add(".mp4", "video/mp4");
                    this.contentTypeConfig.Add(".mpeg", "video/mpeg");
                    this.contentTypeConfig.Add(".mpg", "video/mpeg");
                    this.contentTypeConfig.Add(".qt", "video/quicktime");
                }

                return this.contentTypeConfig;
            }
        }

        protected string getContentType(string fileExtension)
        {
            if (!ContentTypeConfig.ContainsKey(fileExtension))
            {
                throw new ArgumentException("Unsupported content type or unknown content type specified.");
            }

            return ContentTypeConfig[fileExtension];
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}