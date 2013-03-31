using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models.Enquiries
{
    public class PotentialCustomerEnquiry : Enquiry
    {
        [Required]
        [Display(Name = "Title")]
        public Title Title { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Company")]
        public string Company { get; set; }

        [Display(Name = "Email address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Contact")]
        public string ContactNumber { get; set; }
    }
}