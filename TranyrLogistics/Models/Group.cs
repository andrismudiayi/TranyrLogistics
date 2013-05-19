using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranyrLogistics.Models
{
    public abstract class Group
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Group")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime ModifiedDate { get; set; }
    }
}