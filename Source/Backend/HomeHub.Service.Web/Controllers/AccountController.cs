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
    /// This controller is used to link 3rd party accounts with a home hub account.
    /// Every time an account is linked, it resyncs all devices and replaces the previous registration for that account type.
    /// </summary>
    [RoutePrefix("account")]
    [RequireCredentials]
    public class AccountController : ApiController
    {
        /// <summary>
        /// Add a lifx account to this subscription
        /// </summary>
        /// <param name="home">The guid of the home to attach this account to.</param>
        /// <param name="account">
        /// The parameters on the account. In this case, lifx does not really have a good setup.
        /// So you kind of need to go in and create an appkey for us to use. This is a pain in the ass, but it works pretty well.
        /// </param>
        /// <returns>
        /// No content. Anything other than a 204 should be considered an error here.
        /// After linking account, take a look to make sure all new devices were imported properly.
        /// </returns>
        [HttpPut]
        [Route("lifx/{home}")]
        public async Task AddLifxAccount([FromUri]Guid home, [FromBody]LifxAccount account)
        {
            var user = HttpContext.Current.User.Identity.UserId();
            var dict = new Dictionary<string, string> { ["appkey"] = account.AppKey };

            await AccountManager.LinkAccount(user, home, "lifx", dict);
        }


        public async Task GetAccounts([FromUri] Guid home)
        {
            
        }
    }
}
