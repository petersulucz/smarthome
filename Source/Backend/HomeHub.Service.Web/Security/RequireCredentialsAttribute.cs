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

namespace HomeHub.Service.Web.Security
{
    public class RequireCredentialsAttribute : Attribute, IAuthorizationFilter
    {
        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var containsAuth = actionContext.Request.Headers.Contains("sh-name");

            if(false == containsAuth)
            {
                return Task.FromResult(RequireCredentialsAttribute.UnauthorizedResponse());
            }

            var header = actionContext.Request.Headers.GetValues("sh-name").FirstOrDefault();
            var identity = new HubIdentity(header, Guid.Empty);

            var principal = new GenericPrincipal(identity, new[] { "admin" });
            Thread.CurrentPrincipal = principal;
            HttpContext.Current.User = principal;

            return continuation();
        }

        private static HttpResponseMessage UnauthorizedResponse()
        {
            var mesg = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            mesg.ReasonPhrase = "UNAUTHORIZED";

            return mesg;
        }
    }
}