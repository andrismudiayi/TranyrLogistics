using System.Collections.Generic;
using System.Net.Mail;
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

        public static void SendEmail(List<string> sendTo, string subject, string messageBody)
        {
            SmtpClient smtpClient = new SmtpClient("127.0.0.1", 25);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("info@tranyr.com");
            foreach (string to in sendTo)
            {
                mailMessage.To.Add(to);
            }
            mailMessage.Subject = subject;
            mailMessage.Body = messageBody;

            smtpClient.Send(mailMessage);
        }

        protected override void OnMailSent(System.Net.Mail.MailMessage mail)
        {
            base.OnMailSent(mail);
        }
    }
}
