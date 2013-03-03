using System.Web.Mvc;
using ActionMailer.Net.Mvc;
using TranyrLogistics.Models;

namespace TranyrLogistics.Controllers
{
    [Authorize]
    public class EmailTemplateController : MailerBase
    {
        public EmailResult EnquiryVerificationEmail(Enquiry enquiry)
        {
            To.Add(enquiry.EmailAddress);
            From = "no-reply@tranyrlogistics.com";
            Subject = "Enquiry Verification.";
            return Email("EnquiryVerificationEmail", enquiry);
        }

        protected override void OnMailSent(System.Net.Mail.MailMessage mail)
        {
            base.OnMailSent(mail);
        }
    }
}
