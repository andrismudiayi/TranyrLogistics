using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        // GET: /Enquiry/RequestQuote/5

        public ActionResult RequestQuote(int id = 0)
        {
            Enquiry enquiry = db.Enquiries.Find(id);
            enquiry.OriginCountry = db.Countries.Find(enquiry.OriginCountryID);
            enquiry.DestinationCountry = db.Countries.Find(enquiry.DestinationCountryID);

            ViewBag.group_id = new SelectList(db.ServiceProviderGroups, "ID", "Name");

            ViewBag.OriginCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", enquiry.OriginCountryID);
            ViewBag.DestinationCountryID = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", enquiry.DestinationCountryID);

            ViewBag.MessageTemplate = EmailTemplate.PerpareQuoteRequestEmail(enquiry, @"~\views\EmailTemplate\QuoteRequestEmail.html.cshtml");

            return View(enquiry);
        }

        //
        // POST: /Enquiry/Delete/5

        [HttpPost, ActionName("RequestQuote"), ValidateInput(false)]
        public ActionResult RequestQuote(int enquiry_id, int group_id, string subject, string message_body)
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
                    enquiry.QuoteRequested = true;
                }
                catch
                {
                    return RedirectToAction("Error");
                }
            }

            ViewBag.group_id = new SelectList(db.ServiceProviderGroups, "ID", "Name");

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}