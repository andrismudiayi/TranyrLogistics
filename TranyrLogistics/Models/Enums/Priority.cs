using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TranyrLogistics.Models.Enums
{
    public enum Priority
    {
        [Display(Name = "Low")]
        LOW,
        [Display(Name = "Medium")]
        MEDIUM,
        [Display(Name = "High")]
        HIGH
    }
}