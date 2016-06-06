namespace HomeHub.Service.Web
{
    using System.Configuration;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using HomeHub.Service.Common.Data;

    /// <summary>
    /// The web api application
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        /// On start
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DataLayer.Initialize(ConfigurationManager.ConnectionStrings["default"].ConnectionString);
        }
    }
}