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

        [Display(Name = "Reference no.")]
        public string ReferenceNumber { get; set; }

        [Required]
        [Display(Name = "Transport")]
        public Transportation Transport { get; set; }

        [Required]
        [Display(Name = "Origin city")]
        public string OriginCity { get; set; }

        [Display(Name = "Origin country")]
        public Country OriginCountry { get; set; }

        [Required]
        [Display(Name = "Destination address")]
        public string DestinationAddress { get; set; }

        [Required]
        [Display(Name = "Destination city")]
        public string DestinationCity { get; set; }

        [Display(Name = "Destination country")]
        public Country DestinationCountry { get; set; }

        [Required]
        [Display(Name = "Collection point")]
        public string CollectionPoint { get; set; }

        [Display(Name = "Planned collection time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime PlannedCollectionTime { get; set; }

        [Display(Name = "Actual collection time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ActualCollectionTime { get; set; }

        [Required]
        [Display(Name = "Recipient")]
        public string Recipient { get; set; }

        [Display(Name = "Shipping terms")]
        public virtual ShippingTerms ShippingTerms { get; set; }

        [Required]
        [Display(Name = "Shipment category")]
        public ShipmentCategory? Category { get; set; }

        [Required]
        [Display(Name = "Goods description")]
        public string GoodsDescription { get; set; }

        [Display(Name = "Planned ETA")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime PlannedETA { get; set; }

        [Display(Name = "Time of arrival")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ActualTimeOfArrival { get; set; }
        
        [Required]
        [Display(Name = "Number of packages")]
        public int NumberOfPackages { get; set; }

        [Required]
        [Display(Name = "Gross weight (Kg)")]
        public decimal GrossWeight { get; set; }

        [Display(Name = "Volumetric weight")]
        public decimal VolumetricWeight { get; set; }

        [Display(Name = "Insurance required")]
        public bool InsuranceRequired { get; set; }

        [Display(Name = "Master bill no.")]
        public string MasterBillNumber { get; set; }

        [Display(Name = "Way bill no.")]
        public string WayBillNumber { get; set; }

        [Display(Name = "House bill no.")]
        public string HouseBillNumber { get; set; }

        [Display(Name = "Service provider")]
        public virtual ServiceProvider ServiceProvider { get; set; }

        [Required]
        [Display(Name = "Customs entry type")]
        public CustomsEntryType CustomsEntry { get; set; }

        [Required]
        [Display(Name = "ShipShape")]
        public string ShipShape { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? CreateDate { get; set; }

        [Display(Name = "Unresolved issues")]
        public bool HasUnresolvedIssues { get; set; }

        public ShipmentStatus Status { get; set; }

        public virtual DateTime ModifiedDate { get; set; }

        public virtual int CustomerID { get; set; }

        public virtual int? ShippingTermsID { get; set; }

        public virtual int? ServiceProviderID { get; set; }

        public virtual int? OriginCountryID { get; set; }

        public virtual int? DestinationCountryID { get; set; }

        public Shipment()
        {
            ActualTimeOfArrival = DateTime.Now;
            PlannedCollectionTime = DateTime.Now;
            ActualCollectionTime = DateTime.Now;
            PlannedETA = DateTime.Now;
        }
    }
}