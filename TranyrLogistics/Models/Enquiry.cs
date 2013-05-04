using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranyrLogistics.Models.Customers;
using TranyrLogistics.Models.Enquiries;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models
{
    public abstract class Enquiry
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Enquiry type")]
        public EnquiryType EnquiryType { get; set; }

        [Required]
        [Display(Name = "Planned shipment date/time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime PlannedShipmentTime { get; set; }

        [Required]        
        [Display(Name = "Origin city")]
        public string OriginCity { get; set; }

        [Display(Name = "Origin country")]
        public Country OriginCountry { get; set; }

        [Required]
        [Display(Name = "Destination city")]
        public string DestinationCity { get; set; }


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
        public decimal GrossWeight { get; set; }

        [Display(Name = "Volumetric weight")]
        public decimal VolumetricWeight { get; set; }

        [Display(Name = "Insurance required?")]
        public bool InsuranceRequired { get; set; }

        [Display(Name = "Additional Notes")]
        public string Notes { get; set; }

        [Display(Name = "Verified")]
        public bool VerificationSent { get; set; }

        [Display(Name = "Quote req.")]
        public bool QuotationRequested { get; set; }

        [Display(Name = "Quote sent")]
        public bool QuotationSent { get; set; }

        [Display(Name = "Customer confirmation sent")]
        public bool CustomerConfirmationSent { get; set; }

        [Display(Name = "Service provider transport order sent")]
        public bool ProviderTransportOrderSent { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                if (this is PotentialCustomerEnquiry)
                {
                    return ((PotentialCustomerEnquiry)this).FirstName + " " + ((PotentialCustomerEnquiry)this).LastName;
                }
                else if (this is ExistingCustomerEnquiry)
                {
                    return ((ExistingCustomerEnquiry)this).Customer.DisplayName;
                }
                return string.Empty;
            }
        }

        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime ModifiedDate { get; set; }

        public virtual int? OriginCountryID { get; set; }

        public virtual int? DestinationCountryID { get; set; }
    }
}