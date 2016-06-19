namespace HomeHub.Service.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using HomeHub.Service.Common.Data;
    using HomeHub.Service.Common.Security;
    using HomeHub.Service.Web.Models.Manufacturer;
    using HomeHub.Service.Web.Pipeline.Filters;

    /// <summary>
    /// The account controller
    /// </summary>
    [RoutePrefix("account")]
    [RequireCredentials]
    public class AccountController : ApiController
    {
        /// <summary>
        /// Add a lifx account to this subscription
        /// </summary>
        /// <param name="home">The home to attach to</param>
        /// <param name="account">The account parameters</param>
        /// <returns>No content</returns>
        [HttpPut]
        [Route("lifx/{home}")]
        public async Task AddLifxAccount([FromUri]Guid home, [FromBody]LifxAccount account)
        {
            var user = HttpContext.Current.User.Identity.UserId();
            var dict = new Dictionary<string, string> { ["appkey"] = account.AppKey };

            await AccountManager.LinkAccount(user, home, "lifx", dict);
        }
    }
}
