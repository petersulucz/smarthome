using System;
using System.Security.Principal;

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

            throw new NotImplementedException("One of our developers has screwed up and this will not work.");
        }

    }
}
