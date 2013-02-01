﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models
{
    public abstract class Customer
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Customer No.")]
        public string CustomerNumber { get; set; }

        [Required]
        [Display(Name = "Email address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Contact number")]
        public string ContactNumber { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "State/Province")]
        public string StateOrProvince { get; set; }

        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Priority")]
        public Priority Priority { get; set; }

        [Display(Name = "Group")]
        public virtual Group Group { get; set; }

        public virtual int? GroupID { get; set; }
        
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
                    if (((Individual)this).Initial != string.Empty)
                    {
                        return ((Individual)this).FirstName + " " + ((Individual)this).Initial + " " + ((Individual)this).LastName;
                    }
                    else
                    {
                        return ((Individual)this).FirstName + " " + ((Individual)this).LastName;
                    }
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