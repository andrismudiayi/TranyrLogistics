using System.ComponentModel.DataAnnotations;

namespace TranyrLogistics.Models
{
    public class ShippingTerms
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Standard")]
        [Required(ErrorMessage = "*")]
        public string Standard { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}