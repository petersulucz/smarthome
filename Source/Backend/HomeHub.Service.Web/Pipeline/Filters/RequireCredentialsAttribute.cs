namespace HomeHub.Service.Web.Pipeline.Filters
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using HomeHub.Data.Common.Models.Security;
    using HomeHub.Data.Common.Security;
    using HomeHub.Service.Common.Data;
    using HomeHub.Service.Common.Helpers;
    using HomeHub.Service.Common.Security;

    /// <summary>
    /// Attribute to require credentials on api calls
    /// </summary>
    public class RequireCredentialsAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// The name of the authorization header which we look for
        /// </summary>
        public static string AuthorizationHeaderName = "sh-auth";

        /// <summary>Gets or sets a value indicating whether more than one instance of the indicated attribute can be specified for a single program element.</summary>
        /// <returns>true if more than one instance is allowed to be specified; otherwise, false. The default is false.</returns>
        public bool AllowMultiple => false;

        /// <summary>Executes the authorization filter to synchronize.</summary>
        /// <returns>The authorization filter to synchronize.</returns>
        /// <param name="actionContext">The action context.</param>
        /// <param name="cancellationToken">The cancellation token associated with the filter.</param>
        /// <param name="continuation">The continuation.</param>
        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {

            //var isHttps = actionContext.Request.RequestUri.Scheme == Uri.UriSchemeHttps;
            var isHttps = true;

            // Get the custom auth header. YEA THATS RIGHT CUSTOM. SCREW THE SYSTEM
            var containsAuth = actionContext.Request.Headers.Contains(RequireCredentialsAttribute.AuthorizationHeaderName);
            
            // If they didnt include the header, clearly they cant touch stuff
            if (false == containsAuth)
            {
                // If they didnt use https yell at them for being stupid
                return RequireCredentialsAttribute.UnauthorizedResponse();
            }

            // Get the header
            var header = actionContext.Request.Headers.GetValues(RequireCredentialsAttribute.AuthorizationHeaderName).FirstOrDefault();

            if (false == isHttps)
            {
                return new HttpResponseMessage(HttpStatusCode.UpgradeRequired)
                           {
                               Content =
                                   new StringContent(
                                   $"HTTPS REQUIRED. THE TOKEN \'{header}\' HAS BEEN REVOKED BECAUSE WE CAN'T GUARANTEE SOMEONE WONT BE MESSING WITH YOUR STUFF NOW."),
                               ReasonPhrase = "HTTPS REQUIRED"
                           };
            }

            User user = null;

            // Get ip to log
            var ip = IPAddressHelper.GetIPAddress(actionContext.Request);

            try
            {
                user = await DataLayer.Security.GetUser(header, ip, TimeSpan.FromMinutes(5));
            }
            catch (UnauthorizedAccessException)
            {
                return RequireCredentialsAttribute.UnauthorizedResponse();
            }

            var identity = new HubIdentity(user, header);

            var principal = new HubPrincipal(identity);

            // This is mostly here to screw with justin
            if (principal.IsInRole(UserRoles.BLACKLISTED))
            {
                return
                    RequireCredentialsAttribute.BlacklistedResponse(
                        "This account has been blacklisted. All requests will be rejected.");
            }

            Thread.CurrentPrincipal = principal;
            HttpContext.Current.User = principal;

            return await continuation();
        }

        /// <summary>
        /// Returns a response indicating that the user is unauthorized
        /// </summary>
        /// <returns>The unauthorized response</returns>
        private static HttpResponseMessage UnauthorizedResponse()
        {
            var mesg = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized) { ReasonPhrase = "UNAUTHORIZED" };

            return mesg;
        }

        /// <summary>
        /// Return a response indicating a blacklisted account
        /// </summary>
        /// <param name="message">The message to the user</param>
        /// <returns>The blacklisted response</returns>
        private static HttpResponseMessage BlacklistedResponse(string message)
        {
            var mesg = new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                ReasonPhrase = "BLACKLIST",
                Content = new StringContent(message)
            };

            return mesg;
        }
    }
}