using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranyrLogistics.Models
{
    public class ShipmentDocument
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Label")]
        public string Label { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [NotMapped]
        [Display(Name = "File Path")]
        public string FilePath { get; set; }

        public string ActualFileName { get; set; }

        public string FilePathOnDisc { get; set; }

        public string CustomerNumber { get; set; }

        public int ShipmentID { get; set; }

        [Display(Name = "Date")]
        public virtual DateTime? CreateDate { get; set; }
    }
}