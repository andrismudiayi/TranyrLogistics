using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranyrLogistics.Models.Groups
{
    [Table("CustomerGroup")]
    public class CustomerGroup : Group
    {
        public int GroupType { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}