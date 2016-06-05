using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Data.Common.Security
{
    [Flags]
    public enum UserRoles
    {
        /// <summary>
        /// User has no access and will be rejected every timed
        /// </summary>
        BLACKLISTED = 0,

        /// <summary>
        /// Normal access
        /// </summary>
        BASIC = 1,

        /// <summary>
        /// Admin level access
        /// </summary>
        ADMIN = 2
    }
}
