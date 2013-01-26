using System.ComponentModel.DataAnnotations;

namespace TranyrLogistics.Models.Enums
{
    public enum ShipmentCategory
    {
        [Display(Name = "Parcel")]
        PARCEL,
        [Display(Name = "Perishable")]
        PERISHABLE,
        [Display(Name = "Flammable")]
        FLAMMABLE,
        [Display(Name = "Textiles")]
        TEXTILES,
        [Display(Name = "Hazardous materials")]
        HAZARDOUSMATERIALS
    }
}