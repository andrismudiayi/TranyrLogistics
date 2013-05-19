using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
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
        public ActionResult Index(Enquiry.State state = Enquiry.State.OPEN)
        {
            var enquiries = db.Enquiries.Where(x => x.StatusIndex == (int) Enquiry.State.OPEN).Include(c => c.OriginCountry).Include(c => c.DestinationCountry);
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
        // GET: /Enquiry/CreateExistingCustomerEnquiry

        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult CreateExistingCustomerEnquiry(int customer_id = 0)
        {
            if (customer_id > 0)
            {
                Customer customer = db.Customers.Find(customer_id);
                ViewBag.CustomerNumber = customer.CustomerNumber;
            }
            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");

            return View(new ExistingCustomerEnquiry());
        }

        //
        // POST: /Enquiry/Create

        [HttpPost]
        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult CreateExistingCustomerEnquiry(Enquiry enquiry)
        {
            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);
            enquiry.CreateDate = enquiry.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
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
                    enquiry.VerificationSent = true;
                }
                catch { }

                enquiry.CreatedBy = User.Identity.Name;
                enquiry.Status = Enquiry.State.OPEN;

                db.Enquiries.Add(enquiry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");

            return View(enquiry);
        }

        //
        // GET: /Enquiry/CreatePotentialCustomerEnquiry

        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult CreatePotentialCustomerEnquiry()
        {
            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");

            return View(new PotentialCustomerEnquiry());
        }

        //
        // POST: /Enquiry/CreatePotentialCustomerEnquiry

        [HttpPost]
        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult CreatePotentialCustomerEnquiry(Enquiry enquiry)
        {
            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);
            enquiry.CreateDate = enquiry.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    EmailTemplate.Send(
                        ((PotentialCustomerEnquiry)enquiry).EmailAddress,
                        "info@tranyr.com",
                        "Enquiry Verification",
                        EmailTemplate.PrepareVerificationEmail(enquiry, @"~\views\EmailTemplate\EnquiryVerificationEmail.html.cshtml"),
                        true
                    );
                    enquiry.VerificationSent = true;
                }
                catch { }

                enquiry.CreatedBy = User.Identity.Name;
                enquiry.Status = Enquiry.State.OPEN;

                db.Enquiries.Add(enquiry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");

            return View(enquiry);
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
                enquiry.CreatedBy = currentEnquiry.CreatedBy;
                enquiry.AssignedTo = currentEnquiry.AssignedTo;
                enquiry.StatusIndex = currentEnquiry.StatusIndex;
            }
            enquiry.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(enquiry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name");

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

            if (enquiry.PreferedServiceProviderID > 0)
            {
                enquiry.PreferedServiceProvider = db.ServiceProviders.Find(enquiry.PreferedServiceProviderID);
            }

            var customerConfirmations = db.CustomerConfirmations.Where(x => x.EnquiryID == enquiry.ID);
            ViewBag.CustomerConfirmations = customerConfirmations.ToList();

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

            List<string> attachTransportOrder = null;

            if (Request.Files.Count > 0)
            {
                var file = Request.Files["FilePath"];
                var fileName = Path.GetFileName(file.FileName);
                if (fileName != string.Empty)
                {
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

                    customerConfirmation.CreateDate = DateTime.Now;
                    db.CustomerConfirmations.Add(customerConfirmation);

                    attachTransportOrder = new List<string>();
                    attachTransportOrder.Add(customerConfirmation.FilePathOnDisc);
                }
            }
           
            if (ModelState.IsValid)
            {
                try
                {
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

                db.Entry(enquiry).State = EntityState.Modified;
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
            CustomerConfirmation customerTransportOrder = db.CustomerConfirmations.Find(id);

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
                using (TranyrLogisticsDb context = new TranyrLogisticsDb())
                {
                    Enquiry currentEnquiry = context.Enquiries.Find(enquiry.ID);
                    currentEnquiry.PreferedServiceProviderID = service_provider_id;
                    currentEnquiry.TransportOrderSent = true;                    
                    db.Entry(currentEnquiry).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch { }

            return RedirectToAction("CustomerConfirmation", new { id = enquiry.ID });
        }

        //
        // GET: /Enquiry/AssignEnquiryToUser

        [Authorize(Roles = "Customer-Service, Manager")]
        public ActionResult AssignEnquiryToUser(int id, string username)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            if (enquiry == null)
            {
                return HttpNotFound();
            }

            using (TranyrMembershipDb userDb = new TranyrMembershipDb())
            {
                UserProfile userProfile = userDb.UserProfiles.FirstOrDefault(x => x.UserName == username);
                if (!Roles.IsUserInRole(userProfile.UserName, "Manager") || !Roles.IsUserInRole(userProfile.UserName, "Customer-Service"))
                {
                    return HttpNotFound();
                }
            }

            enquiry.AssignedTo = username;

            return this.Edit(enquiry);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}