using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Data.Sql
{
    using System.Net;
    using System.Threading;

    using HomeHub.Common.Exceptions;
    using HomeHub.Common.Security;
    using HomeHub.Common.Trace;
    using HomeHub.Data.Common;
    using HomeHub.Data.Common.Models.Security;
    using HomeHub.Data.Common.Security;

    public partial class SqlDataLayer : ISecurityLayer
    {
        /// <summary>
        ///     Create a user and get a token for it
        /// </summary>
        /// <param name="user">The user info</param>
        /// <returns>The token</returns>
        async Task<AuthenticationToken> ISecurityLayer.CreateUser(CreateUser user, IPAddress ip)
        {
            var salt = PasswordHelper.GenerateRandom(64);
            var passBytes = Encoding.Unicode.GetBytes(user.LoginInfo.Password);

            var hashedPass = PasswordHelper.HashPassword(salt, passBytes);

            var token = PasswordHelper.GenerateRandom(64);

            const UserRoles roles = UserRoles.BASIC;

            return await this.connectionManager.ExecuteSql(
                "auth.createUser",
                collection =>
                {
                    collection.AddWithValue("firstname", user.FirstName);
                    collection.AddWithValue("lastname", user.LastName);
                    collection.AddWithValue("email", user.LoginInfo.Email);
                    collection.AddWithValue("salt", salt);
                    collection.AddWithValue("password", hashedPass);
                    collection.AddWithValue("token", token);
                    collection.AddWithValue("roles", GenerateRolesList(roles));
                    collection.AddWithValue("ip", PasswordHelper.GetIPAddressInSqlForm(ip));
                },
                reader =>
                {
                    ExceptionUtility.ThrowArgumentExceptionIfFalse(reader.Read(), "Could not create user");
                    return new AuthenticationToken
                    {
                        Claims = roles,
                        Token = Convert.ToBase64String(token)
                    };
                },
                this.tokenSource.Token);
        }

        async Task<User> ISecurityLayer.GetUser(AccountToken token, IPAddress ip, TimeSpan expiration)
        {
            var result = await this.connectionManager.ExecuteSql(
                "auth.getUser",
                parameters =>
                {
                    parameters.AddWithValue("token", token.Bytes);
                    parameters.AddWithValue("ip", PasswordHelper.GetIPAddressInSqlForm(ip));
                    parameters.AddWithValue("expiration", (int)expiration.TotalMinutes);
                },
                reader =>
                {
                    if (false == reader.Read())
                    {
                        return null;
                    }
                    else
                    {
                        var user = new User
                        {
                            Id = (Guid)reader["id"],
                            FirstName = (string)reader["first"],
                            LastName = (string)reader["last"],
                            Created = (DateTime)reader["created"]
                        };

                        reader.NextResult();

                        var roles = default(UserRoles);
                        while (reader.Read())
                        {
                            roles |= (UserRoles)Enum.Parse(typeof(UserRoles), (string)reader["claim"]);
                        }

                        user.Roles = roles;

                        return user;
                    }
                },
                this.tokenSource.Token);

            if (null == result)
            {
                throw new UnauthorizedAccessException("Could not find user.");
            }

            return result;
        }

        async Task ISecurityLayer.RevokeToken(Guid user, AccountToken token)
        {
            await
                this.connectionManager.ExecuteSql(
                    "auth.revoketoken",
                    collection =>
                    {
                        collection.AddWithValue("id", user);
                        collection.AddWithValue("token", token.Bytes);
                    },
                    CancellationToken.None);
        }

        async Task ISecurityLayer.RevokeAllToken(Guid user)
        {
            await
                this.connectionManager.ExecuteSql(
                    "auth.revokealltoken",
                    collection => { collection.AddWithValue("id", user); },
                    CancellationToken.None);
        }

        async Task<AuthenticationToken> ISecurityLayer.LoginUser(UserPass userpass, IPAddress ip)
        {
            var id = default(Guid);
            byte[] salt = null;
            byte[] password = null;
            await
                this.connectionManager.ExecuteSql(
                    "auth.getPassword",
                    collection => { collection.AddWithValue("email", userpass.Email); },
                    reader =>
                    {
                        if (false == reader.Read())
                        {
                            HomeHubEventSource.Log.Info($"Could not find user with email: {userpass.Email}");
                            return;
                        }

                        id = (Guid)reader["id"];
                        salt = (byte[])reader["salt"];
                        password = (byte[])reader["password"];
                    },
                    CancellationToken.None);

            if (null == password)
            {
                throw new UnauthorizedAccessException("Authentication failure");
            }

            if (false == PasswordHelper.ArePasswordsEqual(salt, password, userpass.Password))
            {
                throw new UnauthorizedAccessException("Auth failure");
            }

            var newToken = PasswordHelper.GenerateRandom(64);
            var expiration = default(DateTime);
            var assigned = default(DateTime);

            var claims = default(UserRoles);

            await this.connectionManager.ExecuteSql(
                "auth.loginUser",
                collection =>
                {
                    collection.AddWithValue("email", userpass.Email);
                    collection.AddWithValue("password", password);
                    collection.AddWithValue("token", newToken);
                    collection.AddWithValue("ip", PasswordHelper.GetIPAddressInSqlForm(ip));
                },
                reader =>
                {
                    if (false == reader.Read())
                    {
                        ExceptionUtility.ThrowFailureException("Could not insert token into db.");
                    }

                    id = (Guid)reader["id"];
                    newToken = (byte[])reader["token"];
                    assigned = (DateTime)reader["assigned"];

                    reader.NextResult();

                    while (reader.Read())
                    {
                        var role = (UserRoles)Enum.Parse(typeof(UserRoles), (string)reader["claim"]);
                        claims |= role;
                    }
                },
                this.tokenSource.Token);

            return new AuthenticationToken
            {
                Token = Convert.ToBase64String(newToken),
                Claims = claims,
            };
        }

    }
}
