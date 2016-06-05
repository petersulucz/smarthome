using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Models.Security
{
    using HomeHub.Data.Common.Security;

    /// <summary>
    /// An authentication token
    /// </summary>
    public sealed class AuthenticationToken
    {

        /// <summary>
        /// Gets a new authentication token
        /// </summary>
        /// <param name="token"></param>
        public AuthenticationToken(HomeHub.Data.Common.Models.Security.AuthenticationToken token)
        {
            this.Token = token.Token;
            this.Expiration = token.Expiration;

            var claims = new List<string>();

            if (token.Claims == default(UserRoles))
            {
                this.Claims = new[] {default(UserRoles).ToString()};
            }
            else
            {
                foreach (var role in Enum.GetValues(typeof(UserRoles)))
                {
                    if (token.Claims.HasFlag((UserRoles) role))
                    {
                        claims.Add(((UserRoles) role).ToString());
                    }
                }
                this.Claims = claims;
            }
        }

        /// <summary>
        /// The token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The expiration date in UTC
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// The list of claims
        /// </summary>
        public IEnumerable<string> Claims { get; set; }
    }
}
