using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranyrLogistics.Models.Groups
{
    [Table("ServiceProviderGroup")]
    public class ServiceProviderGroup : Group
    {
        public virtual ICollection<ServiceProvider> ServiceProviders { get; set; }
    }
}