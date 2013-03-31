using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models.Enquiries
{
    public class ExistingCustomerEnquiry : Enquiry
    {
        [Required]
        [Display(Name = "Customer No.")]
        public string CustomerNumber { get; set; }

        public int CustomerID { get; set; }

        public Customer Customer { get; set; }
    }
}