﻿using System;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Email address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Contact number")]
        public string ContactNumber { get; set; }

        [Display(Name = "Postal Address")]
        public string PostalAddress { get; set; }

        [Required]
        [Display(Name = "Physical Address")]
        public string Address { get; set; }

        [Display(Name = "State/Province")]
        public string StateOrProvince { get; set; }

        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }
        
        public virtual DateTime? CreateDate { get; set; }

        public virtual DateTime ModifiedDate { get; set; }
    }
}