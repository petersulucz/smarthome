using System.Web;
using System.Web.Mvc;

namespace HomeHub.Service.Web
{
    /// <summary>
    /// The filter config
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Register the global filters
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
