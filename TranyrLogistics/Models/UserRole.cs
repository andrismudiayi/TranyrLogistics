using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranyrLogistics.Models
{
    public class UserRole
    {
        [NotMapped]
        public string UserName { get; set; }

        [NotMapped]
        public string RoleName { get; set; }
    }
}