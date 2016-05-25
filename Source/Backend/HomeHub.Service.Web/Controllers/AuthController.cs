using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HomeHub.Service.Common.Helpers;

namespace HomeHub.Service.Web.Controllers
{
    using System.Threading.Tasks;

    using HomeHub.Service.Common.Data;
    using HomeHub.Service.Common.Models.Security;

    [RoutePrefix("auth")]
    public class AuthController : ApiController
    {
        /// <summary>
        /// Create a user account
        /// </summary>
        /// <param name="user">The user information</param>
        /// <returns>The authentication token for the user</returns>
        [HttpPost]
        [Route("create")]
        public async Task<AuthenticationToken> CreateAccount(User user)
        {
            var ip = IPAddressHelper.GetIPAddress(Request);
            var token = await DataLayer.Security.CreateUser(user.ToSecurityUser(), ip);

            return new AuthenticationToken(token);
        }

        [HttpPost]
        [Route("login")]
        public async Task<AuthenticationToken> LoginUser(UserPass userpass)
        {
            var ip = IPAddressHelper.GetIPAddress(Request);
            try
            {
                var token = await DataLayer.Security.LoginUser(userpass.ToSecurityUserPass(), ip);
                return new AuthenticationToken(token);
            }
            catch (UnauthorizedAccessException)
            {
                var content = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "AUTH FAIL" };
                throw new HttpResponseException(content);
            }
        }

    }
}
