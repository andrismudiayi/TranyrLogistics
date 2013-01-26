using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranyrLogistics.Models
{
    [Table("Company")]
    public class Company : Customer
    {
        [Required]
        [Display(Name = "Company")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "VAT number")]
        public string VatNumber { get; set; }

        [Display(Name = "Postal Address")]
        public string PostalAddress { get; set; }
    }
}