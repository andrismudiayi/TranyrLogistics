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

        [Display(Name = "Enquiry type")]
        [Required(ErrorMessage = "*")]
        public EnquiryType EnquiryType { get; set; }

        [Display(Name = "Transport")]
        [Required(ErrorMessage = "*")]
        public Transportation Transport { get; set; }

        [Display(Name = "Planned shipment date/time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "*")]
        public DateTime PlannedShipmentTime { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Collection point")]
        public string CollectionPoint { get; set; }

        [Display(Name = "Origin city")]
        [Required(ErrorMessage = "*")]
        public string OriginCity { get; set; }

        [Display(Name = "Origin country")]
        public Country OriginCountry { get; set; }

        [Display(Name = "Destination address")]
        [Required(ErrorMessage = "*")]
        public string DestinationAddress { get; set; }

        [Display(Name = "Destination city")]
        [Required(ErrorMessage = "*")]
        public string DestinationCity { get; set; }


        [Display(Name = "Destination country")]
        public Country DestinationCountry { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "*")]
        public ShipmentCategory? Category { get; set; }

        [Display(Name = "Goods description")]
        [Required(ErrorMessage = "*")]
        public string GoodsDescription { get; set; }

        [Display(Name = "Number of packages")]
        [Required(ErrorMessage = "*")]
        public int NumberOfPackages { get; set; }

        [Display(Name = "Gross weight (Kg)")]
        [Required(ErrorMessage = "*")]
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

        [Display(Name = "Transport order sent")]
        public bool TransportOrderSent { get; set; }

        [Display(Name = "Created by")]
        public string CreatedBy { get; set; }

        [Display(Name = "Assigned to")]
        public string AssignedTo { get; set; }

        public virtual int? PreferedServiceProviderID { get; set; }

        [Display(Name = "Prefered Provider")]
        public ServiceProvider PreferedServiceProvider { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual int? OriginCountryID { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual int? DestinationCountryID { get; set; }

        [Display(Name = "Status")]
        [NotMapped]
        public State Status
        {
            get
            {
                return (Enquiry.State)StatusIndex;
            }
            set
            {
                StatusIndex = (int)value;
            }
        }

        public int StatusIndex { get; set; }

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

        public enum State
        {
            [Display(Name = "Closed")]
            CLOSED,
            [Display(Name = "Cancelled")]
            CANCELLED,
            [Display(Name = "Open")]
            OPEN,
        }
    }
}