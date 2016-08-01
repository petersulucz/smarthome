using System;
using System.Security.Principal;
using HomeHub.Data.Common.Security;

namespace HomeHub.Service.Common.Security
{
    using HomeHub.Data.Common.Models.Security;

    public class HubIdentity : IIdentity
    {
        private readonly User user;

        private readonly string token;

        public HubIdentity(User user, string token)
        {
            this.user = user;
            this.token = token;
        }

        public User User => this.user;

        public string AuthenticationType => "GRIDDLE";

        /// <summary>
        /// Returns true if the user has been authenticated
        /// </summary>
        public bool IsAuthenticated => UserId != default(Guid) && false == user.Roles.HasFlag(UserRoles.BLACKLISTED);

        public string Name => $"{this.user.FirstName} {this.user.LastName}";

        public Guid UserId => this.user.Id;

        public string Token => this.token;
    }
}
