using System;

namespace HomeHub.Data.Common.Models.Homes
{
    /// <summary>
    /// The home membership information
    /// </summary>
    public class HomeMembership
    {
        /// <summary>
        /// Initializes an instance of the <see cref="HomeMembership"/> class.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="access">The access level</param>
        public HomeMembership(Guid user, HomeMembershipAccess access)
        {
            this.User = user;
            this.Access = access;
        }

        public Guid User { get; private set; }

        public HomeMembershipAccess Access { get; private set; }
    }
}
