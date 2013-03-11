using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using TranyrLogistics.Models;
using TranyrLogistics.Views.Helpers;

namespace TranyrLogistics.Controllers.Utility
{
    public class EmailTemplate
    {
        public static void Send(string sendTo, string from, string subject, string messageBody, bool isHtml = false)
        {
            List<string> to = new List<string>();
            to.Add(sendTo);

            Send(to, from, subject, messageBody, isHtml);

        }

        public static void Send(List<string> sendTo, string from, string subject, string messageBody, bool isHtml = false)
        {
            SmtpClient smtpClient = new SmtpClient("127.0.0.1", 25);
            //smtpClient.Credentials = new System.Net.NetworkCredential("pamire.fungai@gmail.com", "$@fung41#!");
            //smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = isHtml;
            mailMessage.From = new MailAddress(from);
            foreach (string to in sendTo)
            {
                mailMessage.To.Add(to);
            }
            mailMessage.Subject = subject;
            mailMessage.Body = messageBody;

            smtpClient.Send(mailMessage);
        }

        public static string PerpareQuoteRequestEmail(Enquiry enquiry, string templatePath)
        {
            string readTemplateFile = string.Empty;

            using (StreamReader streamReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(templatePath)))
            {
                readTemplateFile = streamReader.ReadToEnd();

                readTemplateFile = readTemplateFile.Replace("$$CATEGORY$$", HtmlDropDownExtensions.GetEnumDisplay(enquiry.Category));
                readTemplateFile = readTemplateFile.Replace("$$GOODS_DESCRIPTION$$", enquiry.GoodsDescription);
                readTemplateFile = readTemplateFile.Replace("$$PLANNED_SHIPMENT_DATE$$", enquiry.PlannedShipmentTime.ToLongDateString());
                readTemplateFile = readTemplateFile.Replace("$$ORIGIN_CITYY$$", enquiry.OriginCity);
                readTemplateFile = readTemplateFile.Replace("$$ORIGIN_COUNTRY$$", enquiry.OriginCountry.Name);
                readTemplateFile = readTemplateFile.Replace("$$DESTINATION_CITY$$", enquiry.DestinationCity);
                readTemplateFile = readTemplateFile.Replace("$$DESTINATION_COUNTRY$$", enquiry.DestinationCountry.Name);
                readTemplateFile = readTemplateFile.Replace("$$NUMBER_OF_PACKAGES$$", enquiry.NumberOfPackages.ToString());
                readTemplateFile = readTemplateFile.Replace("$$GROSS_WEIGHT$$", enquiry.GrossWeight.ToString());
                readTemplateFile = readTemplateFile.Replace("$$VOLUMETRIC_WEIGHT$$", enquiry.VolumetricWeight.ToString());

                if (enquiry.InsuranceRequired)
                {
                    readTemplateFile = readTemplateFile.Replace("$$INSURANCE_REQUIRED$$", "is insured");
                }
            }

            return readTemplateFile;
        }

        public static string PerpareVerificationEmail(Enquiry enquiry, string templatePath)
        {
            string readTemplateFile = string.Empty;

            using (StreamReader streamReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(templatePath)))
            {
                readTemplateFile = streamReader.ReadToEnd();

                readTemplateFile = readTemplateFile.Replace("$$DISPLAY_NAME$$", enquiry.DisplayName);
                readTemplateFile = readTemplateFile.Replace("$$CATEGORY$$", HtmlDropDownExtensions.GetEnumDisplay(enquiry.Category));
                readTemplateFile = readTemplateFile.Replace("$$GOODS_DESCRIPTION$$", enquiry.GoodsDescription);
                readTemplateFile = readTemplateFile.Replace("$$PLANNED_SHIPMENT_DATE$$", enquiry.PlannedShipmentTime.ToLongDateString());
                readTemplateFile = readTemplateFile.Replace("$$ORIGIN_CITYY$$", enquiry.OriginCity);
                readTemplateFile = readTemplateFile.Replace("$$ORIGIN_COUNTRY$$", enquiry.OriginCountry.Name);
                readTemplateFile = readTemplateFile.Replace("$$DESTINATION_CITY$$", enquiry.DestinationCity);
                readTemplateFile = readTemplateFile.Replace("$$DESTINATION_COUNTRY$$", enquiry.DestinationCountry.Name);
                readTemplateFile = readTemplateFile.Replace("$$NUMBER_OF_PACKAGES$$", enquiry.NumberOfPackages.ToString());
                readTemplateFile = readTemplateFile.Replace("$$GROSS_WEIGHT$$", enquiry.GrossWeight.ToString());
                readTemplateFile = readTemplateFile.Replace("$$VOLUMETRIC_WEIGHT$$", enquiry.VolumetricWeight.ToString());

                if (enquiry.InsuranceRequired)
                {
                    readTemplateFile = readTemplateFile.Replace("$$INSURANCE_REQUIRED$$", "is insured");
                }
            }

            return readTemplateFile;
        }

        public static string FinalizeHtmlEmail(string messageBody)
        {
            string htmlStuff = "<!DOCTYPE html>";
            htmlStuff += "<html><head><meta name='viewport' content='width=device-width' /><title>Enquiry Verification Email</title></head><body>";

            messageBody = htmlStuff + messageBody + "</body></html>";

            return messageBody;
        }
    }
}