using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models
{
    public class Enquiry
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Customer No.")]
        public string CustomerNumber { get; set; }

        [Display(Name = "Title")]
        public Honorific Title { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Company")]
        public string Company { get; set; }

        [Display(Name = "Email address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Contact")]
        public string ContactNumber { get; set; }

        [Display(Name = "Planned shipment date/time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime PlannedShipmentTime { get; set; }

        [Display(Name = "Origin country")]
        public Country OriginCountry { get; set; }


        [Display(Name = "Destination country")]
        public Country DestinationCountry { get; set; }

        [Required]
        [Display(Name = "Category")]
        public ShipmentCategory? Category { get; set; }

        [Required]
        [Display(Name = "Goods description")]
        public string GoodsDescription { get; set; }

        [Required]
        [Display(Name = "Number of packages")]
        public int NumberOfPackages { get; set; }

        [Required]
        [Display(Name = "Gross weight (Kg)")]
        public double GrossWeight { get; set; }

        [Display(Name = "Volumetric weight")]
        public double VolumetricWeight { get; set; }

        [Display(Name = "Insurance required?")]
        public bool InsuranceRequired { get; set; }

        [Display(Name = "Additional Notes")]
        public string Notes { get; set; }

        [Display(Name = "Quote requested")]
        public Boolean QuoteRequested { get; set; }

        [Required]
        [Display(Name = "Quote sent")]
        public Boolean QuoteSent { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime ModifiedDate { get; set; }

        public virtual int? OriginCountryID { get; set; }

        public virtual int? DestinationCountryID { get; set; }
    }
}