using System;
using System.ComponentModel.DataAnnotations;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models
{
    public class Shipment
    {
        [Key]
        public int ID { get; set; }

        public virtual Customer Customer { get; set; }

        [Display(Name = "Reference no.")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Transport")]
        [Required(ErrorMessage = "*")]
        public Transportation Transport { get; set; }

        [Display(Name = "Planned collection time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "*")] 
        public DateTime PlannedCollectionTime { get; set; }

        [Display(Name = "Actual collection time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ActualCollectionTime { get; set; }

        [Display(Name = "Collection point")]
        [Required(ErrorMessage = "*")]
        public string CollectionPoint { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Origin city")]
        public string OriginCity { get; set; }

        [Display(Name = "Origin country")]
        public Country OriginCountry { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Destination address")]
        public string DestinationAddress { get; set; }

        [Display(Name = "Destination city")]
        [Required(ErrorMessage = "*")]
        public string DestinationCity { get; set; }

        [Display(Name = "Destination country")]
        public Country DestinationCountry { get; set; }

        [Display(Name = "Recipient")]
        [Required(ErrorMessage = "*")]
        public string Recipient { get; set; }

        [Display(Name = "Shipping terms")]
        public virtual ShippingTerms ShippingTerms { get; set; }

        [Display(Name = "Shipment category")]
        [Required(ErrorMessage = "*")]
        public ShipmentCategory? Category { get; set; }

        [Display(Name = "Goods description")]
        [Required(ErrorMessage = "*")]
        public string GoodsDescription { get; set; }

        [Display(Name = "Planned ETA")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime PlannedETA { get; set; }

        [Display(Name = "Time of arrival")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ActualTimeOfArrival { get; set; }
                
        [Display(Name = "Number of packages")]
        [Required(ErrorMessage = "*")]
        public int NumberOfPackages { get; set; }

        [Display(Name = "Gross weight (Kg)")]
        [Required(ErrorMessage = "*")]
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

        [Display(Name = "Customs entry type")]
        [Required(ErrorMessage = "*")]
        public CustomsEntryType CustomsEntry { get; set; }

        [Display(Name = "Estimated Value")]
        public string EstimatedValue { get; set; }

        [Display(Name = "ShipShape")]
        public string ShipShape { get; set; }

        [Display(Name = "Client Ref. Number")]
        public string ClientRefNumber { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? CreateDate { get; set; }

        [Display(Name = "Unresolved issues")]
        public bool HasUnresolvedIssues { get; set; }

        [Display(Name = "Created by")]
        public string CreatedBy { get; set; }

        [Display(Name = "Assigned to")]
        public string AssignedTo { get; set; }

        public ShipmentStatus Status { get; set; }

        public virtual DateTime ModifiedDate { get; set; }

        public int? ReferralEnquiryID { get; set; }

        public virtual int CustomerID { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual int? ShippingTermsID { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual int? ServiceProviderID { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual int? OriginCountryID { get; set; }

        [Required(ErrorMessage = "*")]
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