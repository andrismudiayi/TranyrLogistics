using System.ComponentModel.DataAnnotations;

namespace TranyrLogistics.Models.Enums
{
    public enum EnquiryType
    {
        [Display(Name = "Web")]
        WEB,
        [Display(Name = "Phone")]
        PHONE,
        [Display(Name = "Cold Call")]
        COLD_CALL,
        [Display(Name = "Sales")]
        SALES
    }
}