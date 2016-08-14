namespace HomeHub.Service.Web.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using HomeHub.Service.Common.Data;
    using HomeHub.Service.Common.Helpers;
    using HomeHub.Service.Common.Models.Security;
    using HomeHub.Service.Common.Security;
    using HomeHub.Service.Web.Exceptions;
    using HomeHub.Service.Web.Pipeline.Filters;

    /// <summary>
    /// This is the controller for doing authentication related things.
    /// </summary>
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
            if (false == ModelState.IsValid)
            {
                throw new ModelValidationException(this.ModelState);
            }

            var ip = IPAddressHelper.GetIPAddress(this.Request);
            var token = await DataLayer.Security.CreateUser(user.ToSecurityUser(), ip);

            return new AuthenticationToken(token);
        }

        /// <summary>
        /// The login user.
        /// </summary>
        /// <param name="userpass">The user/pass object</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="HttpResponseException">
        /// If shit is unauthorized. Could be a 401
        /// </exception>
        [HttpPost]
        [Route("login")]
        public async Task<AuthenticationToken> LoginUser(UserPass userpass)
        {
            var ip = IPAddressHelper.GetIPAddress(this.Request);
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

        /// <summary>
        /// Revoke the token which was used to make this request
        /// </summary>
        /// <returns>No content. Or probably an internal server error.</returns>
        [HttpDelete]
        [Route("logout")]
        [RequireCredentials]
        public async Task Logout()
        {
            var user = ((HubIdentity)HttpContext.Current.User.Identity).UserId;
            var token = ((HubIdentity)HttpContext.Current.User.Identity).Token;
            await AccountManager.RevokeToken(user, token);
        }

        /// <summary>
        /// Just kill it all. Revoke all tokens which have been assigned to this user.
        /// YOU WILL NEVER ENTER AGAIN.
        /// </summary>
        /// <returns>Nothing.</returns>
        [HttpDelete]
        [Route("revokeall")]
        [RequireCredentials]
        // ReSharper disable once StyleCop.SA1630
        public async Task Revoke()
        {
            var user = ((HubIdentity)HttpContext.Current.User.Identity).UserId;
            await AccountManager.RevokeAllToken(user);
        }
    }
}
