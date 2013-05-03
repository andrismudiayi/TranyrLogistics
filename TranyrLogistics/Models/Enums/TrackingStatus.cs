using System.ComponentModel.DataAnnotations;

namespace TranyrLogistics.Models.Enums
{
    public enum TrackingStatus
    {
        [Display(Name = "To be collected")]
        TOBECOLLECTED,
        [Display(Name = "Loading")]
        LOADING,
        [Display(Name = "Warehouse")]
        WAREHOUSE,
        [Display(Name = "On route")]
        ONROUTE,
        [Display(Name = "Customs")]
        CUSTOMS,
        [Display(Name = "Unloading")]
        UNLOADING,
        [Display(Name = "Delivered")]
        DELIVERED
    }
}