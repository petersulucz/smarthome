using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Security
{
    public static class Extensions
    {
        /// <summary>
        /// Get the user id
        /// </summary>
        /// <param name="self">myself</param>
        /// <returns>A guid</returns>
        public static Guid UserId(this IIdentity self)
        {
            if(self is HubIdentity)
            {
                return ((HubIdentity)self).UserId;
            }

            return Guid.Empty;
        }
    }
}
