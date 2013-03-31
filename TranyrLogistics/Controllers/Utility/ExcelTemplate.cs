using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using TranyrLogistics.Models;
using TranyrLogistics.Models.Customers;
using TranyrLogistics.Models.Enquiries;
using TranyrLogistics.Views.Helpers;

namespace TranyrLogistics.Controllers.Utility
{
    public class ExcelTemplate
    {
        public static FileContentResult GenerateQuote(Enquiry enquiry, string filePath)
        {
            HSSFWorkbook templateWorkbook;

            using (FileStream templateFileStream = new FileStream(System.Web.HttpContext.Current.Server.MapPath(filePath), FileMode.Open, FileAccess.Read))
            {
                templateWorkbook = new HSSFWorkbook(templateFileStream, true);

                ISheet workSheet = templateWorkbook.GetSheet("Estimate");

                // Row 9
                IRow dataRow = workSheet.GetRow(8);

                if (enquiry is PotentialCustomerEnquiry)
                {
                    // Cell A9
                    dataRow.GetCell(0).SetCellValue(((PotentialCustomerEnquiry) enquiry).Company);
                    // Cell B9
                    dataRow.GetCell(1).SetCellValue(((PotentialCustomerEnquiry) enquiry).FirstName);
                    // Cell C9
                    dataRow.GetCell(2).SetCellValue(((PotentialCustomerEnquiry) enquiry).EmailAddress);
                    // Cell D9
                    dataRow.GetCell(3).SetCellValue(((PotentialCustomerEnquiry) enquiry).ContactNumber);
                }
                else if (enquiry is ExistingCustomerEnquiry)
                {
                    // Cell A9
                    dataRow.GetCell(0).SetCellValue(((ExistingCustomerEnquiry) enquiry).Customer.DisplayName);

                    if (((ExistingCustomerEnquiry)enquiry).Customer is Individual)
                    {
                        // Cell B9
                        dataRow.GetCell(1).SetCellValue(((ExistingCustomerEnquiry)enquiry).Customer.DisplayName);
                    }
                    else                    
                    {
                        // Cell B9
                        dataRow.GetCell(1).SetCellValue("-");
                    }
                    // Cell C9
                    dataRow.GetCell(2).SetCellValue(((ExistingCustomerEnquiry) enquiry).Customer.EmailAddress);
                    // Cell D9
                    dataRow.GetCell(3).SetCellValue(((ExistingCustomerEnquiry) enquiry).Customer.ContactNumber);

                }
                // Cell E9
                dataRow.GetCell(4).SetCellValue(HtmlDropDownExtensions.GetEnumDisplay(enquiry.Category));
                // Cell F9
                dataRow.GetCell(5).SetCellValue(enquiry.CreateDate.ToShortDateString());

                // Row 12
                dataRow = workSheet.GetRow(11);
                // Cell A12
                dataRow.GetCell(0).SetCellValue(enquiry.OriginCity + ", " + enquiry.OriginCountry.Name);
                // Cell C12
                dataRow.GetCell(2).SetCellValue(enquiry.DestinationCity + ", " + enquiry.DestinationCountry.Name);

                // Row 19
                dataRow = workSheet.GetRow(18);
                // Cell A19
                dataRow.GetCell(0).SetCellValue(enquiry.NumberOfPackages);
                // Cell A19
                dataRow.GetCell(1).SetCellValue(enquiry.VolumetricWeight.ToString());
                // Cell A19
                dataRow.GetCell(2).SetCellValue(enquiry.GrossWeight.ToString());

                // Enable formula re-calculation
                workSheet.ForceFormulaRecalculation = true;
            }

            MemoryStream memoryStream = new MemoryStream();
            templateWorkbook.Write(memoryStream);

            FileContentResult fileContentResult = new FileContentResult(memoryStream.ToArray(), "application/vnd.ms-excel");
            fileContentResult.FileDownloadName = "TranryEstimate.xls";

            return fileContentResult;
        }
    }
}