using System;
using System.ComponentModel.DataAnnotations;
using TranyrLogistics.Models.Groups;

namespace TranyrLogistics.Models
{
    public class ServiceProvider
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Company")]
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "VAT number")]
        public string VatNumber { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "*")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Contact number")]
        public string ContactNumber { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Emergency number")]
        public string EmergencyNumber { get; set; }

        [Display(Name = "Physical Address")]
        [Required(ErrorMessage = "*")]
        public string PhysicalAddress { get; set; }

        [Display(Name = "Postal Address")]
        public string PostalAddress { get; set; }

        [Display(Name = "Postal code")]
        [Required(ErrorMessage = "*")]
        public string PostalCode { get; set; }

        [Display(Name = "State/Province")]
        [Required(ErrorMessage = "*")]
        public string StateOrProvince { get; set; }

        [Display(Name = "Country")]
        public Country Country { get; set; }

        [Display(Name = "Group")]
        public virtual ServiceProviderGroup ServiceProviderGroup { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual int? ServiceProviderGroupID { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual int? CountryID { get; set; }
        
        public virtual DateTime? CreateDate { get; set; }

        public virtual DateTime ModifiedDate { get; set; }
    }
}