using System.ComponentModel.DataAnnotations;

namespace TranyrLogistics.Models.Enums
{
    public enum Title
    {
        [Display(Name = "Mr")]
        MR,
        [Display(Name = "Ms")]
        MS,
        [Display(Name = "Dr")]
        DR,
        [Display(Name = "Professor")]
        PROFESSOR
    }
}