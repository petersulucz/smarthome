using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Security
{
    using HomeHub.Data.Common.Models.Security;

    public class HubIdentity : IIdentity
    {
        private readonly User user;

        public HubIdentity(User user)
        {
            this.user = user;
        }

        public User User
        {
            get
            {
                return this.user;
            }
        }

        public string AuthenticationType
        {
            get
            {
                return String.Empty;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public string Name
        {
            get
            {
                return $"{this.user.FirstName} {this.user.LastName}";
            }
        }

        public Guid UserId
        {
            get
            {
                return this.user.Id;
            }
        }
    }
}
