using System.ComponentModel.DataAnnotations;

namespace TranyrLogistics.Models.Enums
{
    public enum ShipmentStatus
    {
        [Display(Name = "Closed")]
        CLOSED,
        [Display(Name = "Delivered")]
        DELIVERED,
        [Display(Name = "Open")]
        OPEN
    }
}