using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Security
{
    using System.Security.Principal;

    using HomeHub.Data.Common.Security;

    public class HubPrincipal : IPrincipal
    {
        private UserRoles roles;

        private readonly HubIdentity identity;

        public HubPrincipal(HubIdentity identity)
        {
            this.identity = identity;
            this.roles = identity.User.Roles;
        }

        public IIdentity Identity
        {
            get
            {
                return this.identity;
            }
        }

        public bool IsInRole(string role)
        {
            var ur = (UserRoles)Enum.Parse(typeof(UserRoles), role);
            return this.IsInRole(ur);
        }

        public bool IsInRole(UserRoles role)
        {
            return this.roles.HasFlag(role);
        }
    }
}
