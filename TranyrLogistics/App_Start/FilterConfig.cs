using System.Web;
using System.Web.Mvc;
using TranyrLogistics.Filters;

namespace TranyrLogistics
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}