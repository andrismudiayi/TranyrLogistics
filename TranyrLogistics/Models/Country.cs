using System.ComponentModel.DataAnnotations;

namespace TranyrLogistics.Models
{
    public class Country
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}