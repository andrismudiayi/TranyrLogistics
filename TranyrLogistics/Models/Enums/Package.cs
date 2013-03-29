using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TranyrLogistics.Models.Enums
{
    public enum Package
    {
        [Display(Name = "Air Freight")]
        AIR_FREIGHT,
        [Display(Name = "Sea Freight")]
        SEA_FREIGHT,
        [Display(Name = "Road Freight")]
        ROAD_FREIGHT,
        [Display(Name = "Express")]
        EXPRESS
    }
}