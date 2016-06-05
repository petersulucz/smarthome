using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HomeHub.Common.Exceptions;
using HomeHub.Data.Common.Models.Homes;

namespace HomeHub.Service.Common.Models.Homes
{
    /// <summary>
    /// The home membership model
    /// </summary>
    public class HomeMembershipModel
    {
        /// <summary>
        /// Constructor for asp
        /// </summary>
        public HomeMembershipModel()
        {
            // FOR ASP
        }

        /// <summary>
        /// Intializes a new instance of the <see cref="HomeMembershipModel"/> class.
        /// </summary>
        /// <param name="membership">The membership object</param>
        public HomeMembershipModel(HomeMembership membership)
        {
            this.User = membership.User;
            this.Roles = HomeMembershipModel.GetRoles(membership.Access);
        }

        /// <summary>
        /// The user id
        /// </summary>
        public Guid User { get; set; }

        /// <summary>
        /// The list of roles
        /// </summary>
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// Gets the home membership model from this set
        /// </summary>
        /// <returns>The home membership model</returns>
        public HomeMembership ToHomeMembership()
        {
            return new HomeMembership(this.User, HomeMembershipModel.GetHomeMembershipAccessFromStrings(this.Roles));
        }

        /// <summary>
        /// Get Home membership access result from a list of string roles
        /// </summary>
        /// <param name="roles">The list of roles</param>
        /// <returns>A home membership access bit field</returns>
        private static HomeMembershipAccess GetHomeMembershipAccessFromStrings(IEnumerable<string> roles)
        {
            var result = default(HomeMembershipAccess);
            foreach (var role in roles)
            {
                var parsed = default(HomeMembershipAccess);
                if (false == Enum.TryParse<HomeMembershipAccess>(role, true, out parsed))
                {
                    throw new FailureException($"Could not parse membership arguement \'{role}\'", "INVALID DATA", HttpStatusCode.BadRequest);
                }
                result |= parsed;

            }
            return result;
        }

        /// <summary>
        /// Get the roles list of membership
        /// </summary>
        /// <param name="roles">The roles</param>
        /// <returns>A list of the string roles</returns>
        private static IEnumerable<string> GetRoles(HomeMembershipAccess roles)
        {
            // omg.
            return (from access in Enum.GetValues(typeof(HomeMembershipAccess)).Cast<HomeMembershipAccess>() where roles.HasFlag(access) select access.ToString()).ToList();
        }
    }
}
