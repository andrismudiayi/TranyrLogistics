using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranyrLogistics.Models.Enums;
using TranyrLogistics.Models.Customers;
using TranyrLogistics.Models.Groups;

namespace TranyrLogistics.Models
{
    public abstract class Customer
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Customer No.")]
        public string CustomerNumber { get; set; }

        [Display(Name = "Email address")]
        [EmailAddress]
        [Required(ErrorMessage = "*")]
        public string EmailAddress { get; set; }

        [Display(Name = "Contact number")]
        [Required(ErrorMessage = "*")]
        public string ContactNumber { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

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

        [Display(Name = "Priority")]
        [Required(ErrorMessage = "*")]
        public Priority Priority { get; set; }

        [Display(Name = "Group")]
        public virtual CustomerGroup CustomerGroup { get; set; }

        public string Notes { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual int? CustomerGroupID { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual int? CountryID { get; set; }
        
        public virtual DateTime? CreateDate { get; set; }

        public virtual DateTime ModifiedDate { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                if (this is Individual)
                {
                    return ((Individual)this).FirstName + " " + ((Individual)this).LastName;
                }
                else if (this is Company)
                {
                    return ((Company)this).Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}