using System.ComponentModel.DataAnnotations;

namespace TranyrLogistics.Models.Enums
{
    public enum CustomsEntryType
    {
        [Display(Name = "DP")]
        DP,
        [Display(Name = "WE")]
        WE,
        [Display(Name = "WH")]
        WH,
        [Display(Name = "XE")]
        XE,
        [Display(Name = "Other")]
        OTHER
    }
}