using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Data.Common.Models.Security
{
    using HomeHub.Data.Common.Security;

    public class User
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; }
        public UserRoles Roles { get; set; }
    }
}
