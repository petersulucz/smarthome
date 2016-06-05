namespace HomeHub.Service.Common.Security
{
    using System;
    using System.Security.Principal;

    using HomeHub.Data.Common.Security;

    public class HubPrincipal : IPrincipal
    {
        private readonly HubIdentity identity;

        private readonly UserRoles roles;

        public HubPrincipal(HubIdentity identity)
        {
            this.identity = identity;
            this.roles = identity.User.Roles;
        }

        public IIdentity Identity => this.identity;

        public bool IsInRole(string role)
        {
            var ur = (UserRoles)Enum.Parse(typeof(UserRoles), role);
            return this.IsInRole(ur);
        }

        public bool IsInRole(UserRoles role)
        {
            if (role == default(UserRoles))
            {
                return this.roles == role;
            }

            return this.roles.HasFlag(role);
        }
    }
}