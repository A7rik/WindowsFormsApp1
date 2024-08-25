using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;

namespace MvcApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            Logger.Info("Application is starting...");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Logger.Info("Application started successfully.");
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            Logger.Error(exception, "An unhandled exception occurred.");
        }

        protected void Application_End()
        {
            Logger.Info("Application is shutting down...");
        }
    }
}
