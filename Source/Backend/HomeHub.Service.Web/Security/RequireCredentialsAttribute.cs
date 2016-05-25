using HomeHub.Service.Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using HomeHub.Service.Common.Helpers;

namespace HomeHub.Service.Web.Security
{
    using HomeHub.Data.Common.Models.Security;
    using HomeHub.Service.Common.Data;

    public class RequireCredentialsAttribute : Attribute, IAuthorizationFilter
    {
        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var containsAuth = actionContext.Request.Headers.Contains("sh-auth");
            

            if (false == containsAuth)
            {
                return RequireCredentialsAttribute.UnauthorizedResponse();
            }

            var header = actionContext.Request.Headers.GetValues("sh-auth").FirstOrDefault();

            User user = null;

            var ip = IPAddressHelper.GetIPAddress(actionContext.Request);

            try
            {
                user = await DataLayer.Security.GetUser(header, ip);
            }
            catch (UnauthorizedAccessException)
            {
                return RequireCredentialsAttribute.UnauthorizedResponse();
            }

            var identity = new HubIdentity(user);

            var principal = new HubPrincipal(identity);
            Thread.CurrentPrincipal = principal;
            HttpContext.Current.User = principal;

            return await continuation();
        }

        private static HttpResponseMessage UnauthorizedResponse()
        {
            var mesg = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            mesg.ReasonPhrase = "UNAUTHORIZED";

            return mesg;
        }
    }
}