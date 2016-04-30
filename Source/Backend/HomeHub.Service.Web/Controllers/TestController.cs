using HomeHub.Service.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HomeHub.Service.Web.Controllers
{
    /// <summary>
    /// This is the test controller. 
    /// </summary>
    [RequireCredentialsAttribute]
    [RoutePrefix("test")]
    public class TestController : ApiController
    {
        /// <summary>
        /// Get a test string
        /// </summary>
        /// <returns>A test string</returns>
        [HttpGet]
        [Route("")]
        public string Test()
        {
            var name = HttpContext.Current.User.Identity.Name;
            return $"Hi {name}: Up.. Current time {DateTime.UtcNow}. ";
        }
    }
}
