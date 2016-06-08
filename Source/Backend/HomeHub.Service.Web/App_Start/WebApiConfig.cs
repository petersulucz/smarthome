using System.Web.Http;
using System.Web.Http.Cors;

namespace HomeHub.Service.Web
{
    using HomeHub.Service.Web.Pipeline.Filters;
    using HomeHub.Service.Web.Pipeline.Handlers;

    /// <summary>
    /// The main config
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="config">Config object</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Make sure failure exceptions are ok
            config.Filters.Add(new FailureExceptionFilter());

            config.MessageHandlers.Add(new GriddleResponseHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
