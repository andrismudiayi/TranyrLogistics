using System.ComponentModel.DataAnnotations;

namespace TranyrLogistics.Models.Enums
{
    public enum Transportation
    {
        [Display(Name = "Air")]
        AIR,
        [Display(Name = "Road")]
        ROAD,
        [Display(Name = "Sea")]
        SEA,
        [Display(Name = "Air + Road")]
        AIR_ROAD,
        [Display(Name = "Air + Sea")]
        AIR_SEA,
        [Display(Name = "Road + Sea")]
        ROAD_SEA
    }
}