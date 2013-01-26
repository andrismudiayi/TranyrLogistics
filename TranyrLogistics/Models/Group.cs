using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TranyrLogistics.Models
{
    public class Group
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Group")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime ModifiedDate { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}