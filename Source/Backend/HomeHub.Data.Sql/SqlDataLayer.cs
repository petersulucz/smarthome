using HomeHub.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHub.Common.Devices;
using HomeHub.Data.Common.Models.Security;

namespace HomeHub.Data.Sql
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;

    using HomeHub.Common.Exceptions;
    using HomeHub.Common.Trace;
    using HomeHub.Data.Common.Security;

    public class SqlDataLayer : IDataLayer, ISecurityLayer
    {

        Task<IEnumerable<Device>> IDataLayer.GetAllDevices(Guid user)
        {

            var lst = new[]
                    {
                        new Light(Guid.Empty, "device 1"),
                        new Light(Guid.Empty, "device 2")
                    };
            return Task.FromResult((IEnumerable<Device>)(lst));
        }

        async Task<AuthenticationToken> ISecurityLayer.LoginUser(UserPass userpass)
        {
            var id = default(Guid);
            byte[] salt = null;
            byte[] password = null;
            await SqlConnectionManager.ExecuteSql(
                "auth.getPassword",
                collection =>
                    {
                        collection.AddWithValue("email", userpass.Email);
                    },
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
            DateTime expiration = default(DateTime);
            DateTime assigned = default(DateTime);

            var claims = default(UserRoles);

            await SqlConnectionManager.ExecuteSql(
                "auth.loginUser",
                collection =>
                    {
                        collection.AddWithValue("email", userpass.Email);
                        collection.AddWithValue("password", password);
                        collection.AddWithValue("token", newToken);
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
                        expiration = (DateTime)reader["expiration"];

                        reader.NextResult();

                        while (reader.Read())
                        {
                            var role = (UserRoles)Enum.Parse(typeof(UserRoles), (string)reader["claim"]);
                            claims |= role;
                        }
                    },
                CancellationToken.None);

            return new AuthenticationToken()
                       {
                           Token = Convert.ToBase64String(newToken),
                           Claims = claims,
                           Expiration = expiration
                       };
        }

        /// <summary>
        /// Create a user and get a token for it
        /// </summary>
        /// <param name="user">The user info</param>
        /// <returns>The token</returns>
        async Task<AuthenticationToken> ISecurityLayer.CreateUser(CreateUser user)
        {
            var salt = PasswordHelper.GenerateRandom(64);
            var passBytes = Encoding.Unicode.GetBytes(user.LoginInfo.Password);

            var hashedPass = PasswordHelper.HashPassword(salt, passBytes);

            var token = PasswordHelper.GenerateRandom(64);

            var roles = UserRoles.ADMIN | UserRoles.BASIC;

            AuthenticationToken authToken = null;

            await SqlConnectionManager.ExecuteSql(
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
                    },
                reader =>
                    {
                        ExceptionUtility.ThrowArgumentExceptionIfFalse(reader.Read(), "Could not create user");
                        authToken = new AuthenticationToken()
                                        {
                                            Claims = roles,
                                            Expiration = (DateTime)reader["expiration"],
                                            Token = Convert.ToBase64String(token)
                                        };
                    },
                CancellationToken.None);

            return authToken;
        }

        /// <summary>
        /// Get a roles list datatable
        /// </summary>
        /// <param name="roles">The list of roles</param>
        /// <returns>The dt containing the roles</returns>
        private static DataTable GenerateRolesList(UserRoles roles)
        {
            var datatable = new DataTable();
            datatable.Columns.Add("claim");

            foreach (var role in Enum.GetValues(typeof(UserRoles)))
            {
                if (roles.HasFlag((UserRoles)role))
                {
                    datatable.Rows.Add(((UserRoles)role).ToString());
                }
            }

            return datatable;
        }

        async Task<User> ISecurityLayer.GetUser(string token)
        {
            var bytes = Convert.FromBase64String(token);

            var found = true;
            User user = null;
            

            await SqlConnectionManager.ExecuteSql(
                "auth.getUser",
                parameters =>
                    {
                        parameters.AddWithValue("token", bytes);
                    },
                reader =>
                    {
                        if (false == reader.Read())
                        {
                            found = false;
                        }
                        else
                        {
                            user = new User()
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
                        }
                    },
                CancellationToken.None);

            if (false == found)
            {
                throw new UnauthorizedAccessException("Could not find user.");
            }

            return user;
        }
    }
}
