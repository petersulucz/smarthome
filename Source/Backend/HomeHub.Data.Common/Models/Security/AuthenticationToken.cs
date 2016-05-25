using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Data.Common.Models.Security
{
    using HomeHub.Data.Common.Security;

    public class AuthenticationToken
    {
        /// <summary>
        /// The token
        /// </summary>
        public virtual string Token { get; set; }

        /// <summary>
        /// The expiration date in UTC
        /// </summary>
        public virtual DateTime Expiration { get; set; }

        /// <summary>
        /// The list of claims
        /// </summary>
        public virtual UserRoles Claims { get; set; }

    }
}
