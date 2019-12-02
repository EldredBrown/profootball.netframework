using System.Data.Entity.Infrastructure.Interception;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EldredBrown.ProFootball.AspNet.MvcWebApp.DAL;
using EldredBrown.ProFootball.AspNet.MvcWebApp.App_Start;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DbInterception.Add(new SchoolInterceptorTransientErrors());
            DbInterception.Add(new SchoolInterceptorLogging());
            DependencyInjectionConfig.Register();

            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
