using System;
using System.ComponentModel.DataAnnotations;
using TranyrLogistics.Models.Enums;

namespace TranyrLogistics.Models
{
    public class ShipmentTracker
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Status")]
        public TrackingStatus TrackingStatus { get; set; }

        [Display(Name = "Time")]
        public virtual DateTime? CreateDate { get; set; }

        public int ShipmentID { get; set; }
    }
}