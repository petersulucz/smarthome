namespace HomeHub.Data.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using HomeHub.Common.Devices;
    using HomeHub.Common.Exceptions;
    using HomeHub.Common.Trace;
    using HomeHub.Data.Common;
    using HomeHub.Data.Common.Models;
    using HomeHub.Data.Common.Models.Security;
    using HomeHub.Data.Common.Security;

    public class SqlDataLayer : IDataLayer, ISecurityLayer, IDisposable
    {
        private readonly SqlConnectionManager connectionManager;

        private readonly CancellationTokenSource tokenSource;

        public SqlDataLayer(string connectionString)
        {
            this.connectionManager = new SqlConnectionManager(connectionString);
            this.tokenSource = new CancellationTokenSource();
        }

        /// <summary>
        ///     Dispose everything
        /// </summary>
        public void Dispose()
        {
            this.tokenSource.Dispose();
        }

        async Task<Home> IDataLayer.CreateHome(Home home, Guid user)
        {
            return await this.connectionManager.ExecuteSql(
                "hub.createhome", 
                collection =>
                    {
                        collection.AddWithValue("name", home.Name);
                        collection.AddWithValue("user", user);
                    },
                reader =>
                    {
                        reader.Read();
                        return SqlDataLayer.ReadHome(reader);
                    }, 
                this.tokenSource.Token);
        }

        async Task<IEnumerable<Home>> IDataLayer.GetHomes(Guid user)
        {
            return await this.connectionManager.ExecuteSql(
                "hub.gethomes",
                collection =>
                    {
                        collection.AddWithValue("user", user);
                    },
                reader =>
                    {
                        var result = new List<Home>();
                        while (reader.Read())
                        {
                            result.Add(SqlDataLayer.ReadHome(reader));
                        }
                        return result;
                    },
                this.tokenSource.Token);
        }

        Task<IEnumerable<Device>> IDataLayer.GetAllDevices(Guid user)
        {
            var lst = new[] { new Light(Guid.Empty, "device 1"), new Light(Guid.Empty, "device 2") };
            return Task.FromResult((IEnumerable<Device>)lst);
        }

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

            const UserRoles Roles = UserRoles.ADMIN | UserRoles.BASIC;

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
                        collection.AddWithValue("roles", GenerateRolesList(Roles));
                        collection.AddWithValue("ip", PasswordHelper.GetIPAddressInSqlForm(ip));
                    }, 
                reader =>
                    {
                        ExceptionUtility.ThrowArgumentExceptionIfFalse(reader.Read(), "Could not create user");
                        return new AuthenticationToken
                                        {
                                            Claims = Roles, 
                                            Expiration = (DateTime)reader["expiration"], 
                                            Token = Convert.ToBase64String(token)
                                        };
                    }, 
                this.tokenSource.Token);
        }

        async Task<User> ISecurityLayer.GetUser(string token, IPAddress ip)
        {
            var bytes = Convert.FromBase64String(token);

            var result = await this.connectionManager.ExecuteSql(
                "auth.getUser", 
                parameters =>
                    {
                        parameters.AddWithValue("token", bytes);
                        parameters.AddWithValue("ip", PasswordHelper.GetIPAddressInSqlForm(ip));
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
                        expiration = (DateTime)reader["expiration"];

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
                           Expiration = expiration
                       };
        }

        /// <summary>
        ///     Get a roles list datatable
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

        private static Home ReadHome(IDataRecord reader)
        {
            return new Home((string)reader["name"], (DateTime)reader["created"], (Guid)reader["id"]);
        }
    }
}