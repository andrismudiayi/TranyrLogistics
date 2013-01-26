using System.ComponentModel.DataAnnotations;

namespace TranyrLogistics.Models.Enums
{
    public enum Transport
    {
        [Display(Name = "Air")]
        AIR,
        [Display(Name = "Road")]
        ROAD,
        [Display(Name = "Sea")]
        SEA,
        [Display(Name = "Air + Road")]
        AIR_ROAD,
        [Display(Name = "Air + SEA")]
        AIR_SEA,
        [Display(Name = "Road + SEA")]
        ROAD_SEA,
        [Display(Name = "Air + Road + Sea")]
        AIR_ROAD_SEA
    }
}