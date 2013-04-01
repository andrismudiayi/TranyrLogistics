using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TranyrLogistics.Controllers.Utility;
using TranyrLogistics.Models;
using TranyrLogistics.Models.Enquiries;

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
            var enquiryList = enquiries.ToList();
            foreach (Enquiry enquiry in enquiryList)
            {
                if (enquiry is ExistingCustomerEnquiry)
                {
                    var customer = db.Customers.Where(x => x.CustomerNumber == ((ExistingCustomerEnquiry)enquiry).CustomerNumber).FirstOrDefault();
                    ((ExistingCustomerEnquiry)enquiry).Customer = customer;
                }
            }

            return View(enquiryList);
        }

        //
        // GET: /Enquiry/Details/5

        public ActionResult Details(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            if (enquiry == null)
            {
                return HttpNotFound();
            }
            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);
            if (enquiry is ExistingCustomerEnquiry)
            {
                var customer = db.Customers.Where(x => x.CustomerNumber == ((ExistingCustomerEnquiry)enquiry).CustomerNumber).FirstOrDefault();
                ((ExistingCustomerEnquiry)enquiry).Customer = customer;
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
            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);
            enquiry.CreateDate = enquiry.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    if (enquiry is PotentialCustomerEnquiry)
                    {
                        EmailTemplate.Send(
                            ((PotentialCustomerEnquiry)enquiry).EmailAddress,
                            "info@tranyr.com",
                            "Enquiry Verification",
                            EmailTemplate.PerpareVerificationEmail(enquiry, @"~\views\EmailTemplate\EnquiryVerificationEmail.html.cshtml"),
                            true
                        );
                    }
                    else if (enquiry is ExistingCustomerEnquiry)
                    {
                        var customer = db.Customers.FirstOrDefault(x => x.CustomerNumber == ((ExistingCustomerEnquiry)enquiry).CustomerNumber);

                        ((ExistingCustomerEnquiry)enquiry).CustomerID = customer.ID;
                        ((ExistingCustomerEnquiry)enquiry).Customer = customer;

                        EmailTemplate.Send(
                            ((ExistingCustomerEnquiry)enquiry).Customer.EmailAddress,
                            "info@tranyr.com",
                            "Enquiry Verification",
                            EmailTemplate.PerpareVerificationEmail(enquiry, @"~\views\EmailTemplate\EnquiryVerificationEmail.html.cshtml"),
                            true
                        );
                    }
                    enquiry.VerificationSent = true;
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
            using (TranyrLogisticsDb db = new TranyrLogisticsDb())
            {
                var currentEnquiry = db.Enquiries.Find(enquiry.ID);
                if (enquiry is ExistingCustomerEnquiry)
                {
                    ((ExistingCustomerEnquiry)enquiry).CustomerID = ((ExistingCustomerEnquiry)currentEnquiry).CustomerID;
                }

                enquiry.VerificationSent = currentEnquiry.VerificationSent;
                enquiry.QuotationRequested = currentEnquiry.QuotationRequested;
                enquiry.QuotationSent = currentEnquiry.QuotationSent;
                enquiry.CreateDate = currentEnquiry.CreateDate;
            }
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
            if (enquiry is ExistingCustomerEnquiry)
            {
                ((ExistingCustomerEnquiry)enquiry).Customer = db.Customers.Where(x => x.CustomerNumber == ((ExistingCustomerEnquiry)enquiry).CustomerNumber).FirstOrDefault();
            }

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
            if (enquiry is ExistingCustomerEnquiry)
            {
                ((ExistingCustomerEnquiry)enquiry).Customer = db.Customers.Where(x => x.CustomerNumber == ((ExistingCustomerEnquiry)enquiry).CustomerNumber).FirstOrDefault();
            }

            return ExcelTemplate.GenerateQuote(enquiry, @"\views\EmailTemplate\QuoteTemplate.xls");
        }

        //
        //GET: /Enquiry/SendQuotation/5

        public ActionResult SendQuotation(int id)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            if (enquiry is ExistingCustomerEnquiry)
            {
                ((ExistingCustomerEnquiry)enquiry).Customer = db.Customers.Where(x => x.CustomerNumber == ((ExistingCustomerEnquiry)enquiry).CustomerNumber).FirstOrDefault();
            }

            Quotation quotation = new Quotation();
            quotation.EnquiryID = id;

            if (enquiry is PotentialCustomerEnquiry)
            {
                ViewBag.ToCustomer = ((PotentialCustomerEnquiry)enquiry).DisplayName;
                ViewBag.ToEmail = ((PotentialCustomerEnquiry)enquiry).EmailAddress;
            }
            else if (enquiry is ExistingCustomerEnquiry)
            {
                ViewBag.ToCustomer = ((ExistingCustomerEnquiry)enquiry).Customer.DisplayName;
                ViewBag.ToEmail = ((ExistingCustomerEnquiry)enquiry).Customer.EmailAddress;
            }

            ViewBag.MessageTemplate = EmailTemplate.PerpareSendQuotationEmail(enquiry, @"~\views\EmailTemplate\SendQuotationEmail.html.cshtml");

            return View(quotation);
        }

        //
        //POST: /Enquiry/SendQuotation

        [HttpPost, ActionName("SendQuotation"), ValidateInput(false)]
        public ActionResult SendQuotation(Quotation quotation, string subject, string message_body)
        {
            Enquiry enquiry = db.Enquiries.Find(quotation.EnquiryID);
            if (enquiry is ExistingCustomerEnquiry)
            {
                ((ExistingCustomerEnquiry)enquiry).Customer = db.Customers.Where(x => x.CustomerNumber == ((ExistingCustomerEnquiry)enquiry).CustomerNumber).FirstOrDefault();
            }

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

                    if (enquiry is PotentialCustomerEnquiry)
                    {
                        EmailTemplate.Send(((PotentialCustomerEnquiry)enquiry).EmailAddress, "info@tranyr.com", subject, EmailTemplate.FinalizeHtmlEmail(message_body), true, attachQuote);
                    }
                    else if (enquiry is ExistingCustomerEnquiry)
                    {
                        EmailTemplate.Send(((ExistingCustomerEnquiry)enquiry).Customer.EmailAddress, "info@tranyr.com", subject, EmailTemplate.FinalizeHtmlEmail(message_body), true, attachQuote);
                    }

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