namespace HomeHub.Data.Common
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using HomeHub.Common.Security;
    using HomeHub.Data.Common.Models.Security;

    public interface ISecurityLayer
    {
        Task<AuthenticationToken> CreateUser(CreateUser user, IPAddress ip);

        Task<AuthenticationToken> LoginUser(UserPass userpass, IPAddress ip);

        Task<User> GetUser(AccountToken token, IPAddress ip, TimeSpan expiration);

        Task RevokeToken(Guid user, AccountToken token);

        Task RevokeAllToken(Guid user);
    }
}
