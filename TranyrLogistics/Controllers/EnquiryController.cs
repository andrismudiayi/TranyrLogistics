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
    public class EnquiryController : Controller
    {
        private TranyrLogisticsDb db = new TranyrLogisticsDb();

        //
        // GET: /Enquiry/

        [Authorize(Roles = "Customer-Service, Finance, Manager, Operator")]
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

        [Authorize(Roles = "Customer-Service, Finance, Manager, Operator")]
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

        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult Create()
        {
            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            return View();
        }

        //
        // POST: /Enquiry/Create

        [HttpPost]
        [Authorize(Roles = "Customer-Service, Manager")]
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
                            EmailTemplate.PrepareVerificationEmail(enquiry, @"~\views\EmailTemplate\EnquiryVerificationEmail.html.cshtml"),
                            true
                        );
                    }
                    else if (enquiry is ExistingCustomerEnquiry)
                    {
                        var customer = db.Customers.FirstOrDefault(x => x.CustomerNumber == ((ExistingCustomerEnquiry)enquiry).CustomerNumber);
                        if (customer == null)
                        {
                            return RedirectToAction("NotFound");
                        }

                        ((ExistingCustomerEnquiry)enquiry).CustomerID = customer.ID;
                        ((ExistingCustomerEnquiry)enquiry).Customer = customer;

                        EmailTemplate.Send(
                            ((ExistingCustomerEnquiry)enquiry).Customer.EmailAddress,
                            "info@tranyr.com",
                            "Enquiry Verification",
                            EmailTemplate.PrepareVerificationEmail(enquiry, @"~\views\EmailTemplate\EnquiryVerificationEmail.html.cshtml"),
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

        [Authorize(Roles = "Customer-Service, Manager")]
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
        [Authorize(Roles = "Customer-Service, Manager")]
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

        [Authorize(Roles = "Manager")]
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
        [Authorize(Roles = "Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            db.Enquiries.Remove(enquiry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Enquiry/ConfirmTransportOrder/5

        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult CustomerConfirmation(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);
            if (enquiry is ExistingCustomerEnquiry)
            {
                ((ExistingCustomerEnquiry)enquiry).Customer = db.Customers.Where(x => x.CustomerNumber == ((ExistingCustomerEnquiry)enquiry).CustomerNumber).FirstOrDefault();
            }

            var customerTransportOrders = db.CustomerTransportOrders.Where(x => x.EnquiryID == enquiry.ID);
            ViewBag.CustomerTransportOrders = customerTransportOrders.ToList();

            return View(enquiry);
        }

        //
        // GET: /Enquiry/RequestQuotation/5

        [Authorize(Roles = "Customer-Service, Manager")]
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
        [Authorize(Roles = "Customer-Service, Manager")]
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

        [Authorize(Roles = "Customer-Service, Manager")]
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

        [Authorize(Roles = "Customer-Service, Manager")]
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

        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult SendQuotation(int id)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            if (enquiry is ExistingCustomerEnquiry)
            {
                ((ExistingCustomerEnquiry)enquiry).Customer = db.Customers.Where(x => x.CustomerNumber == ((ExistingCustomerEnquiry)enquiry).CustomerNumber).FirstOrDefault();
            }

            Quotation quotation = new Quotation();
            quotation.EnquiryID = enquiry.ID;

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
        [Authorize(Roles = "Customer-Service, Manager")]
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

        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult DownloadQuotation(int id = 0)
        {
            Quotation quotation = db.Quotations.Find(id);

            return File(quotation.FilePathOnDisc,
                FileContent.GetType(Path.GetExtension(quotation.ActualFileName)),
                string.Format("{0}", quotation.ActualFileName)
            );
        }

        //
        // GET: /Enquiry/SendCustomerConfirmation/5

        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult SendCustomerConfirmation(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            if (enquiry is PotentialCustomerEnquiry)
            {
                return RedirectToAction("Error");
            }

            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);

            ViewBag.group_id = new SelectList(db.ServiceProviderGroups, "ID", "Name");

            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", enquiry.OriginCountryID);
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", enquiry.DestinationCountryID);
 
            CustomerConfirmation customerTransportOrder = new CustomerConfirmation();
            customerTransportOrder.EnquiryID = enquiry.ID;
                        
            var customer = db.Customers.Find(((ExistingCustomerEnquiry)enquiry).CustomerID);
            ((ExistingCustomerEnquiry)enquiry).Customer = customer;

            ViewBag.ToCustomer = ((ExistingCustomerEnquiry)enquiry).Customer.DisplayName;
            ViewBag.ToEmail = ((ExistingCustomerEnquiry)enquiry).Customer.EmailAddress;

            ViewBag.MessageTemplate = EmailTemplate.PrepareCustomerConfirmationEmail(enquiry, @"~\views\EmailTemplate\CustomerConfirmationEmail.html.cshtml");

            return View(customerTransportOrder);
        }

        //
        //POST: /Enquiry/SendCustomerConfirmation

        [HttpPost, ActionName("SendCustomerConfirmation"), ValidateInput(false)]
        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult SendCustomerConfirmation(CustomerConfirmation customerConfirmation, string subject, string message_body)
        {
            Enquiry enquiry = db.Enquiries.Find(customerConfirmation.EnquiryID);
            if (enquiry is ExistingCustomerEnquiry)
            {
                ((ExistingCustomerEnquiry)enquiry).Customer = db.Customers.Where(x => x.CustomerNumber == ((ExistingCustomerEnquiry)enquiry).CustomerNumber).FirstOrDefault();
            }
            else
            {
                return RedirectToAction("Error");
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
                customerConfirmation.ActualFileName = fileName;
                customerConfirmation.FilePathOnDisc = savedFileName;
            }

            customerConfirmation.CreateDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.CustomerTransportOrders.Add(customerConfirmation);
                db.SaveChanges();

                try
                {
                    List<string> attachTransportOrder = new List<string>();
                    attachTransportOrder.Add(customerConfirmation.FilePathOnDisc);

                    if (enquiry is PotentialCustomerEnquiry)
                    {
                        EmailTemplate.Send(((PotentialCustomerEnquiry)enquiry).EmailAddress, "info@tranyr.com", subject, EmailTemplate.FinalizeHtmlEmail(message_body), true, attachTransportOrder);
                    }
                    else if (enquiry is ExistingCustomerEnquiry)
                    {
                        EmailTemplate.Send(((ExistingCustomerEnquiry)enquiry).Customer.EmailAddress, "info@tranyr.com", subject, EmailTemplate.FinalizeHtmlEmail(message_body), true, attachTransportOrder);
                    }

                    enquiry.CustomerConfirmationSent = true;
                                        
                    EmailTemplate.Send(
                        "finance@tranyr.com",
                        "info@tranyr.com",
                        "Customer Shipment Confirmation Sent",
                        EmailTemplate.PrepareInternalCustomerConfirmationEmail(enquiry, @"~\views\EmailTemplate\InternalCustomerConfirmationEmail.html.cshtml"),
                        true
                    );
                }
                catch { }

                db.SaveChanges();

                return RedirectToAction("CustomerConfirmation", new { id = enquiry.ID });
            }

            return View(customerConfirmation);
        }

        //
        // GET: /Enquiry/DownloadTransportOrder/5

        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult DownloadTransportOrder(int id = 0)
        {
            CustomerConfirmation customerTransportOrder = db.CustomerTransportOrders.Find(id);

            return File(customerTransportOrder.FilePathOnDisc,
                FileContent.GetType(Path.GetExtension(customerTransportOrder.ActualFileName)),
                string.Format("{0}", customerTransportOrder.ActualFileName)
            );
        }

        //
        // GET: /Enquiry/SendTransportOrder/5

        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult SendTransportOrder(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            if (enquiry is PotentialCustomerEnquiry)
            {
                return RedirectToAction("Error");
            }

            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);

            ViewBag.service_provider_id = new SelectList(db.ServiceProviders, "ID", "Name");

            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", enquiry.OriginCountryID);
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", enquiry.DestinationCountryID);

            ViewBag.MessageTemplate = EmailTemplate.PrepareCustomerConfirmationEmail(enquiry, @"~\views\EmailTemplate\TransportOrderEmail.html.cshtml");

            return View(enquiry);
        }

        //
        //POST: /Enquiry/SendTransportOrder

        [HttpPost, ActionName("SendTransportOrder"), ValidateInput(false)]
        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult SendTransportOrder(Enquiry enquiry, int service_provider_id, string subject, string message_body)
        {
            ServiceProvider serviceProvider = db.ServiceProviders.Find(service_provider_id);
                        
            try
            {
                EmailTemplate.Send(serviceProvider.EmailAddress, "info@tranyr.com", subject, EmailTemplate.FinalizeHtmlEmail(message_body), true);
                enquiry.ProviderTransportOrderSent = true;
            }
            catch
            { }

            db.SaveChanges();

            return RedirectToAction("CustomerConfirmation", new { id = enquiry.ID });
        }

        //
        // GET: /Enquiry/ReassignEnquiry/5

        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult ReassignEnquiry(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            if (enquiry is ExistingCustomerEnquiry)
            {
                return RedirectToAction("Error");
            }
            return View(enquiry);
        }

        //
        //POST: /Enquiry/ReassignEnquiry

        [HttpPost, ActionName("ReassignEnquiry"), ValidateInput(false)]
        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult ReassignEnquiry(Enquiry enquiry, string customer_number)
        {
            enquiry = db.Enquiries.Find(enquiry.ID);

            if (enquiry is ExistingCustomerEnquiry)
            {
                return RedirectToAction("Error");
            }

            Customer customer = db.Customers.FirstOrDefault(x => x.CustomerNumber == customer_number);
            if (customer == null)
            {
                return RedirectToAction("Error");
            }
            
            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);

            ExistingCustomerEnquiry existingCustomerEnquiry = new ExistingCustomerEnquiry();
            existingCustomerEnquiry.CustomerID = customer.ID;
            existingCustomerEnquiry.Customer = customer;
            existingCustomerEnquiry.CustomerNumber = customer.CustomerNumber;
            existingCustomerEnquiry.EnquiryType = enquiry.EnquiryType;
            existingCustomerEnquiry.PlannedShipmentTime = enquiry.PlannedShipmentTime;
            existingCustomerEnquiry.OriginCity = enquiry.OriginCity;
            existingCustomerEnquiry.OriginCountry = enquiry.OriginCountry;
            existingCustomerEnquiry.OriginCountryID = enquiry.OriginCountryID;
            existingCustomerEnquiry.DestinationCity = enquiry.DestinationCity;
            existingCustomerEnquiry.DestinationCountry = enquiry.DestinationCountry;
            existingCustomerEnquiry.DestinationCountryID = enquiry.DestinationCountryID;
            existingCustomerEnquiry.Category = enquiry.Category;
            existingCustomerEnquiry.GoodsDescription = enquiry.GoodsDescription;
            existingCustomerEnquiry.NumberOfPackages = enquiry.NumberOfPackages;
            existingCustomerEnquiry.GrossWeight = enquiry.GrossWeight;
            existingCustomerEnquiry.VolumetricWeight = enquiry.VolumetricWeight;
            existingCustomerEnquiry.InsuranceRequired = enquiry.InsuranceRequired;
            existingCustomerEnquiry.Notes = enquiry.Notes;
            existingCustomerEnquiry.VerificationSent = enquiry.VerificationSent;
            existingCustomerEnquiry.QuotationRequested = enquiry.QuotationRequested;
            existingCustomerEnquiry.QuotationSent = enquiry.QuotationSent;
            existingCustomerEnquiry.CustomerConfirmationSent = enquiry.CustomerConfirmationSent;
            existingCustomerEnquiry.ProviderTransportOrderSent = enquiry.ProviderTransportOrderSent;
            existingCustomerEnquiry.CreateDate = enquiry.CreateDate;
            existingCustomerEnquiry.ModifiedDate = enquiry.ModifiedDate;
            existingCustomerEnquiry.VerificationSent = enquiry.VerificationSent;
            existingCustomerEnquiry.ID = enquiry.ID;

            db.Entry(enquiry).State = EntityState.Deleted;
            db.Entry(existingCustomerEnquiry).State = EntityState.Added;
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