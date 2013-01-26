using System;
using System.ComponentModel.DataAnnotations;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models
{
    public class Shipment
    {
        [Key]
        public int ShipmentID { get; set; }

        public virtual Customer Customer { get; set; }

        [Display(Name = "Reference No.")]
        public string ReferenceNumber { get; set; }

        [Required]
        [Display(Name = "Transport")]
        public Transportation Transport { get; set; }

        [Required]
        [Display(Name = "City of origin")]
        public string OriginCity { get; set; }

        [Required]
        [Display(Name = "Country of origin")]
        public string OriginCountry { get; set; }

        [Required]
        [Display(Name = "Destination address")]
        public string DestinationAddress { get; set; }

        [Required]
        [Display(Name = "Destination city")]
        public string DestinationCity { get; set; }

        [Required]
        [Display(Name = "Destination country")]
        public string DestinationCountry { get; set; }

        [Required]
        [Display(Name = "Pick up point")]
        public string PickUpPoint { get; set; }

        [Required]
        [Display(Name = "Recipient")]
        public string Recipient { get; set; }

        [Display(Name = "Shipping Terms")]
        public virtual ShippingTerms ShippingTerms { get; set; }

        [Required]
        [Display(Name = "Shipment category")]
        public ShipmentCategory? Category { get; set; }

        [Required]
        [Display(Name = "Shipment description")]
        public string ShipmentDescription { get; set; }

        [Display(Name = "ETA")]
        [DataType(DataType.Date)]
        public DateTime ShipmentEta { get; set; }
        
        [Required]
        [Display(Name = "Number of packages")]
        public int NumberOfPackages { get; set; }

        [Required]
        [Display(Name = "Gross Weight (Kg)")]
        public double GrossWeight { get; set; }

        [Display(Name = "Volume")]
        public double Volume { get; set; }

        [Display(Name = "Insurance required")]
        public bool InsuranceRequired { get; set; }

        [Display(Name = "Master Bill No.")]
        public string MasterBillNumber { get; set; }

        [Display(Name = "Sub-Master Bill No.")]
        public string SubMasterBillNumber { get; set; }

        [Display(Name = "House Bill No.")]
        public string HouseBillNumber { get; set; }

        [Display(Name = "Service Provider")]
        public virtual ServiceProvider ServiceProvider { get; set; }

        [Display(Name = "Date")]
        public virtual DateTime? CreateDate { get; set; }

        public virtual DateTime ModifiedDate { get; set; }

        public virtual int CustomerID { get; set; }

        public virtual int? ShippingTermsID { get; set; }

        public virtual int? ServiceProviderID { get; set; }
    }
}