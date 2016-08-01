using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Data.Common
{
    using HomeHub.Data.Common.Models.Security;

    public interface ISecurityLayer
    {
        Task<AuthenticationToken> CreateUser(CreateUser user, IPAddress ip);

        Task<AuthenticationToken> LoginUser(UserPass userpass, IPAddress ip);

        Task<User> GetUser(string token, IPAddress ip, TimeSpan expiration);

        Task RevokeToken(Guid user, string token);

        Task RevokeAllToken(Guid user);
    }
}
