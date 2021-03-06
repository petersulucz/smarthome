﻿using System;
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
        /// Initializes a new instance of the <see cref="AuthenticationToken"/> class. 
        /// Gets a new authentication token
        /// </summary>
        /// <param name="token">The token we are using.</param>
        public AuthenticationToken(HomeHub.Data.Common.Models.Security.AuthenticationToken token)
        {
            this.Token = token.Token;

            var claims = new List<string>();

            if (token.Claims == default(UserRoles))
            {
                this.Claims = new[] {default(UserRoles).ToString()};
            }
            else
            {
                claims.AddRange(
                    from UserRoles role in Enum.GetValues(typeof(UserRoles))
                    where default(UserRoles) != role
                    where token.Claims.HasFlag(role)
                    select role.ToString());

                this.Claims = claims;
            }
        }

        /// <summary>
        /// The actual token. This is a base64 encoded binary string. Its 512 bits of binary, which is 64 bytes.
        /// Not exactly sure what this translates to, but its probably something like 86 characters.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The list of claims for the user.
        /// </summary>
        public IEnumerable<string> Claims { get; set; }
    }
}
