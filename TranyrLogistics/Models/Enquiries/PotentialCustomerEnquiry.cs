using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models.Enquiries
{
    public class PotentialCustomerEnquiry : Enquiry
    {
        [Display(Name = "Title")]
        [Required(ErrorMessage = "*")]
        public Title Title { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }

        [Display(Name = "Company")]
        public string Company { get; set; }

        [Display(Name = "Email address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Display(Name = "Contact")]
        [Required(ErrorMessage = "*")]
        public string ContactNumber { get; set; }
    }
}