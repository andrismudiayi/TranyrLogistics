using System;
using System.ComponentModel.DataAnnotations;
using TranyrLogistics.Models.Groups;

namespace TranyrLogistics.Models
{
    public class ServiceProvider
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Company")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "VAT number")]
        public string VatNumber { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Contact number")]
        public string ContactNumber { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Emergency number")]
        public string EmergencyNumber { get; set; }

        [Required]
        [Display(Name = "Physical Address")]
        public string PhysicalAddress { get; set; }

        [Display(Name = "Postal Address")]
        public string PostalAddress { get; set; }

        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        [Display(Name = "State/Province")]
        public string StateOrProvince { get; set; }

        [Display(Name = "Country")]
        public Country Country { get; set; }

        [Display(Name = "Group")]
        public virtual ServiceProviderGroup ServiceProviderGroup { get; set; }

        public virtual int? ServiceProviderGroupID { get; set; }

        public virtual int? CountryID { get; set; }
        
        public virtual DateTime? CreateDate { get; set; }

        public virtual DateTime ModifiedDate { get; set; }
    }
}