using System.ComponentModel.DataAnnotations;

namespace TranyrLogistics.Models
{
    public class ShippingTerms
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Standard")]
        public string Standard { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}