using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranyrLogistics.Models.Customers
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

        [Display(Name = "Importers code")]
        public string ImportersCode { get; set; }
    }
}