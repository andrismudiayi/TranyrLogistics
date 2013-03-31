using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models
{
    public class Quotation
    {
        [Key]
        public int ID { get; set; }

        [NotMapped]
        [Display(Name = "File Path")]
        public string FilePath { get; set; }

        public string ActualFileName { get; set; }

        public string FilePathOnDisc { get; set; }

        public int EnquiryID { get; set; }

        [Display(Name = "Date")]
        public virtual DateTime? CreateDate { get; set; }
    }
}