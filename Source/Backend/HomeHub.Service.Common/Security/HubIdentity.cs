using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Security
{
    public class HubIdentity : IIdentity
    {
        private Guid userId;
        private string name;

        public HubIdentity(string name, Guid user)
        {
            this.userId = user;
            this.name = name;
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
                return this.name;
            }
        }

        public Guid UserId
        {
            get
            {
                return this.userId;
            }
        }
    }
}
