using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using TranyrLogistics.Models;
using TranyrLogistics.Models.Customers;
using TranyrLogistics.Models.Enquiries;
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

        public static void Send(string sendTo, string from, string subject, string messageBody, bool isHtml = false, List<string> attachmentFiles = null)
        {
            List<string> to = new List<string>();
            to.Add(sendTo);

            Send(to, from, subject, messageBody, isHtml, attachmentFiles);
        }

        public static void Send(List<string> sendTo, string from, string subject, string messageBody, bool isHtml = false, List<string> attachmentFiles = null)
        {
            SmtpClient smtpClient = new SmtpClient("127.0.0.1", 25);
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

            if (attachmentFiles != null && attachmentFiles.Count > 0)
            {
                foreach (string attachmentFile in attachmentFiles)
                {
                    Attachment attachment = new Attachment(attachmentFile, MediaTypeNames.Application.Octet);
                    mailMessage.Attachments.Add(attachment);
                }
            }

            smtpClient.Send(mailMessage);
        }

        public static string PerpareQuotationRequestEmail(Enquiry enquiry, string templatePath)
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

        public static string PrepareCustomerConfirmationEmail(Enquiry enquiry, string templatePath)
        {
            string readTemplateFile = string.Empty;

            using (StreamReader streamReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(templatePath)))
            {
                readTemplateFile = streamReader.ReadToEnd();

                if (enquiry is PotentialCustomerEnquiry)
                {
                    readTemplateFile = readTemplateFile.Replace("$$FIRST_NAME$$", ((PotentialCustomerEnquiry)enquiry).FirstName);
                }
                else if (enquiry is ExistingCustomerEnquiry)
                {
                    if (((ExistingCustomerEnquiry)enquiry).Customer is Individual)
                    {
                        readTemplateFile = readTemplateFile.Replace("$$FIRST_NAME$$", (((Individual)((ExistingCustomerEnquiry)enquiry).Customer)).FirstName);
                    }
                    else if (((ExistingCustomerEnquiry)enquiry).Customer is Company)
                    {
                        readTemplateFile = readTemplateFile.Replace("$$FIRST_NAME$$", "Sir/Madam");
                    }
                }

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

        public static string PrepareInternalCustomerConfirmationEmail(Enquiry enquiry, string templatePath)
        {
            string readTemplateFile = string.Empty;

            using (StreamReader streamReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(templatePath)))
            {
                readTemplateFile = streamReader.ReadToEnd();

                if (enquiry is ExistingCustomerEnquiry)
                {
                    readTemplateFile = readTemplateFile.Replace("$$CUSTOMER_NUMBER$$", ((ExistingCustomerEnquiry)enquiry).CustomerID.ToString());
                }
                readTemplateFile = readTemplateFile.Replace("$$DISPLAY_NAME$$", enquiry.DisplayName);
            }

            return readTemplateFile;
        }

        public static string PrepareVerificationEmail(Enquiry enquiry, string templatePath)
        {
            string readTemplateFile = string.Empty;

            using (StreamReader streamReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(templatePath)))
            {
                readTemplateFile = streamReader.ReadToEnd();

                if (enquiry is PotentialCustomerEnquiry)
                {
                    readTemplateFile = readTemplateFile.Replace("$$FIRST_NAME$$", ((PotentialCustomerEnquiry)enquiry).FirstName);
                }
                else if (enquiry is ExistingCustomerEnquiry)
                {
                    if (((ExistingCustomerEnquiry)enquiry).Customer is Individual)
                    {
                        readTemplateFile = readTemplateFile.Replace("$$FIRST_NAME$$", (((Individual)((ExistingCustomerEnquiry)enquiry).Customer)).FirstName);
                    }
                    else if (((ExistingCustomerEnquiry)enquiry).Customer is Company)
                    {
                        readTemplateFile = readTemplateFile.Replace("$$FIRST_NAME$$", "Sir/Madam");
                    }
                }
                
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

        public static string PerpareSendQuotationEmail(Enquiry enquiry, string templatePath)
        {
            string readTemplateFile = string.Empty;

            using (StreamReader streamReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(templatePath)))
            {
                readTemplateFile = streamReader.ReadToEnd();

                if (enquiry is PotentialCustomerEnquiry)
                {
                    readTemplateFile = readTemplateFile.Replace("$$FIRST_NAME$$", ((PotentialCustomerEnquiry)enquiry).FirstName);
                }
                else if (enquiry is ExistingCustomerEnquiry)
                {
                    if (((ExistingCustomerEnquiry)enquiry).Customer is Individual)
                    {
                        readTemplateFile = readTemplateFile.Replace("$$FIRST_NAME$$", (((Individual)((ExistingCustomerEnquiry)enquiry).Customer)).FirstName);
                    }
                    else if (((ExistingCustomerEnquiry)enquiry).Customer is Company)
                    {
                        readTemplateFile = readTemplateFile.Replace("$$FIRST_NAME$$", "Sir/Madam");
                    }
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