using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models.Customers
{
    [Table("Individual")]
    public class Individual : Customer
    {
        [Required]
        [Display(Name = "Title")]
        public Honorific? Title { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Initial")]
        public string Initial { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Identity")]
        public string IdentityNumber { get; set; }

        [Display(Name = "Tax number")]
        public string TaxNumber { get; set; }
    }
}