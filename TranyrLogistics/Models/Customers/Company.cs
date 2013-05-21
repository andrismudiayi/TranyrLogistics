using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranyrLogistics.Models.Customers
{
    [Table("Company")]
    public class Company : Customer
    {
        [Display(Name = "Company")]
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

        [Display(Name = "VAT number")]
        [Required(ErrorMessage = "*")]
        public string VatNumber { get; set; }

        [Display(Name = "Importers code")]
        public string ImportersCode { get; set; }
    }
}