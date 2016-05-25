using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Data.Common
{
    using HomeHub.Data.Common.Models.Security;

    public interface ISecurityLayer
    {
        Task<AuthenticationToken> CreateUser(CreateUser user);

        Task<AuthenticationToken> LoginUser(UserPass userpass);

        Task<User> GetUser(string token);
    }
}
