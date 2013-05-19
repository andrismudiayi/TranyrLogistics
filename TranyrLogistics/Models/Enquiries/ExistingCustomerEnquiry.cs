using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models.Enquiries
{
    public class ExistingCustomerEnquiry : Enquiry
    {
        [Display(Name = "Customer No.")]
        [Required(ErrorMessage = "*")]
        public string CustomerNumber { get; set; }

        public int CustomerID { get; set; }

        public Customer Customer { get; set; }
    }
}