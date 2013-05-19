using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models.Customers
{
    [Table("Individual")]
    public class Individual : Customer
    {
        [Display(Name = "Title")]
        [Required(ErrorMessage = "*")]
        public Title? Title { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }

        [Display(Name = "Identity")]
        [Required(ErrorMessage = "*")]
        public string IdentityNumber { get; set; }

        [Display(Name = "Tax number")]
        [Required(ErrorMessage = "*")]
        public string TaxNumber { get; set; }
    }
}