using System;
using System.ComponentModel.DataAnnotations;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models
{
    public class Enquiry
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Title")]
        public Honorific Title { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Contact number")]
        public string ContactNumber { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }

        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime ModifiedDate { get; set; }
    }
}