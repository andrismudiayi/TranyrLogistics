using System;
using System.Data.Entity;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TranyrLogistics.Models;
using TranyrLogistics.Models.CustomModelBinders;
using WebMatrix.WebData;

namespace TranyrLogistics
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            ModelBinders.Binders.DefaultBinder = new CustomerModelBinder();

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TranyrLogisticsDb>());
            WebApiConfig.Register(GlobalConfiguration.Configuration);            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}