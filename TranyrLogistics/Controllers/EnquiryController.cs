using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TranyrLogistics.Controllers.Utility;
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
            var enquiries = db.Enquiries.Include(c => c.OriginCountry).Include(c => c.DestinationCountry);
            return View(enquiries.ToList());
        }

        //
        // GET: /Enquiry/Details/5

        public ActionResult Details(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);
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
            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
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
                enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
                enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);

                try
                {
                    string message_body = EmailTemplate.PerpareVerificationEmail(enquiry, @"~\views\EmailTemplate\EnquiryVerificationEmail.html.cshtml");
                    EmailTemplate.Send(enquiry.EmailAddress, "info@tranyr.com", "Enquiry Verification", message_body, true);
                    enquiry.Verified = true;
                }
                catch { }

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
            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", enquiry.OriginCountryID);
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", enquiry.DestinationCountryID);
            
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
            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);
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

        //
        // GET: /Enquiry/RequestQuotation/5

        public ActionResult RequestQuotation(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);

            ViewBag.group_id = new SelectList(db.ServiceProviderGroups, "ID", "Name");

            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", enquiry.OriginCountryID);
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", enquiry.DestinationCountryID);

            ViewBag.MessageTemplate = EmailTemplate.PerpareQuotationRequestEmail(enquiry, @"~\views\EmailTemplate\QuotationRequestEmail.html.cshtml");

            return View(enquiry);
        }

        //
        // POST: /Enquiry/RequestQuotation

        [HttpPost, ActionName("RequestQuotation"), ValidateInput(false)]
        public ActionResult RequestQuotation(int enquiry_id, int group_id, string subject, string message_body)
        {
            Enquiry enquiry = db.Enquiries.Find(enquiry_id);

            var serviceProviders = db.ServiceProviders.Where(x => x.ServiceProviderGroupID == group_id);

            List<string> sendTo = new List<string>();
            foreach (ServiceProvider serviceProvider in serviceProviders)
            {
                sendTo.Add(serviceProvider.EmailAddress);
            }

            if (sendTo.Count > 0)
            {
                try
                {
                    EmailTemplate.Send(sendTo, "info@tranyr.com", subject, EmailTemplate.FinalizeHtmlEmail(message_body), true);
                    enquiry.QuotationRequested = true;
                }
                catch
                {
                    return RedirectToAction("Error");
                }

                db.SaveChanges();
            }

            ViewBag.group_id = new SelectList(db.ServiceProviderGroups, "ID", "Name");

            return RedirectToAction("Index");
        }

        //
        // GET: /Enquiry/GenerateQuote/5

        public ActionResult GenerateQuotation(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);

            var quotations = db.Quotations.Where(x => x.EnquiryID == enquiry.ID);
            ViewBag.Quotations = quotations.ToList();

            return View(enquiry);
        }

        //
        //GET: /Enquiry/GenerateQuoteTemplate/5

        public ActionResult GenerateQuotationTemplate(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);

            return ExcelTemplate.GenerateQuote(enquiry, @"\views\EmailTemplate\QuoteTemplate.xls");
        }

        //
        //GET: /Enquiry/SendQuotation/5

        public ActionResult SendQuotation(int id)
        {            
            Enquiry enquiry = db.Enquiries.Find(id);
            Quotation quotation = new Quotation();
            quotation.EnquiryID = id;

            ViewBag.ToEmail = enquiry.EmailAddress;
            ViewBag.MessageTemplate = EmailTemplate.PerpareSendQuotationEmail(enquiry, @"~\views\EmailTemplate\SendQuotationEmail.html.cshtml");

            return View(quotation);
        }

        //
        //POST: /Enquiry/SendQuotation

        [HttpPost, ActionName("SendQuotation"), ValidateInput(false)]
        public ActionResult SendQuotation(Quotation quotation, string subject, string message_body)
        {
            Enquiry enquiry = db.Enquiries.Find(quotation.EnquiryID);

            if (Request.Files.Count > 0)
            {
                var file = Request.Files["FilePath"];
                var fileName = Path.GetFileName(file.FileName);
                string uploadDirectoryPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory +
                    @"Uploads\EnquiryDocs\" + enquiry.ID + @"\"
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
                quotation.ActualFileName = fileName;
                quotation.FilePathOnDisc = savedFileName;
            }

            quotation.CreateDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Quotations.Add(quotation);
                db.SaveChanges();

                try
                {
                    List<string> attachQuote = new List<string>();
                    attachQuote.Add(quotation.FilePathOnDisc);

                    EmailTemplate.Send(enquiry.EmailAddress, "info@tranyr.com", subject, EmailTemplate.FinalizeHtmlEmail(message_body), true, attachQuote);
                    enquiry.QuotationSent = true;
                }
                catch
                {
                    return RedirectToAction("Error");
                }

                db.SaveChanges();

                return RedirectToAction("GenerateQuotation", new { id = enquiry.ID });
            }

            return View(quotation);
        }

        //
        // GET: /Enquiry/DownloadQuotation/5

        public ActionResult DownloadQuotation(int id = 0)
        {
            Quotation quotation = db.Quotations.Find(id);

            return File(quotation.FilePathOnDisc,
                FileContent.GetType(Path.GetExtension(quotation.ActualFileName)),
                string.Format("{0}", quotation.ActualFileName)
            );
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}