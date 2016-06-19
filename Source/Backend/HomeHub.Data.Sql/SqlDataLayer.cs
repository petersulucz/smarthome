using HomeHub.Data.Common.Models.Homes;

namespace HomeHub.Data.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using HomeHub.Adapters.Common;
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

        /// <summary>
        /// Create a new device
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="home">The home</param>
        /// <param name="description">The description</param>
        /// <param name="definition">The definition</param>
        /// <param name="meta">The metadata about the device</param>
        /// <returns>The new device</returns>
        public async Task<Device> CreateDevice(string name, Guid home, string description, Guid definition, string meta)
        {
            return await this.connectionManager.ExecuteSql(
                "hub.adddevice",
                collection =>
                    {
                        collection.AddWithValue("name", name);
                        collection.AddWithValue("home", home);
                        collection.AddWithValue("description", description);
                        collection.AddWithValue("definition", definition);
                        collection.AddWithValue("meta", meta);
                    },
                reader =>
                    {
                        // Get the guid for the new device
                        reader.Read();
                        var id = (Guid)reader["id"];

                        reader.NextResult();

                        // Read the functions
                        var functions = SqlDataLayer.GetDeviceFunctions(reader);

                        reader.NextResult();

                        // read the device definition
                        reader.Read();
                        var defintion = new DeviceDefinition(
                            definition,
                            (string)reader["manufacturer"],
                            (string)reader["product"],
                            (DeviceType)reader["type"],
                            functions[definition]);

                        return new Device(id, home, name, description, defintion, meta);
                    },
                this.tokenSource.Token);
        }

        /// <summary>
        /// Get the device definitions
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, IEnumerable<DeviceDefinition>>> GetDefinitions()
        {
            var devices = await this.connectionManager.ExecuteSql(
                "hub.getdefinitions",
                collection => { },
                reader =>
                    {
                        var func = SqlDataLayer.GetDeviceFunctions(reader);

                        reader.NextResult();


                        var definitions = new Dictionary<string, IEnumerable<DeviceDefinition>>();
                        while (reader.Read())
                        {
                            var id = (Guid)reader["id"];
                            var manufacturer = (string)reader["name"];
                            var definition = new DeviceDefinition(
                                id,
                                manufacturer,
                                (string)reader["product"],
                                (DeviceType)reader["type"],
                                func[id]);

                            if (false == definitions.ContainsKey(manufacturer))
                            {
                                definitions.Add(manufacturer, new List<DeviceDefinition>());
                            }

                            ((List<DeviceDefinition>)definitions[manufacturer]).Add(definition);
                        }

                        return definitions;
                    },
                this.tokenSource.Token);

            return devices;
        }

        /// <summary>
        /// Create a new home
        /// </summary>
        /// <param name="home">The home to create</param>
        /// <param name="user">The user creating the home</param>
        /// <returns></returns>
        public async Task<Home> CreateHome(Home home, Guid user)
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

        /// <summary>
        /// Get the homes for a user
        /// </summary>
        /// <param name="user">The user id</param>
        /// <returns>A list of all homes for this user</returns>
        public async Task<IEnumerable<Home>> GetHomes(Guid user)
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

        /// <summary>
        /// Get all of the users attached to a house
        /// </summary>
        /// <param name="home">The home to check</param>
        /// <param name="user">The user calling the method</param>
        /// <returns>The list of home memberships</returns>
        public async Task<IEnumerable<HomeMembership>> GetHomeUsers(Guid home, Guid user)
        {
            return await this.connectionManager.ExecuteSql("hub.gethomeusers", collection =>
            {
                collection.AddWithValue("home", home);
                collection.AddWithValue("user", user);
            }, reader =>
            {

                var members = new List<HomeMembership>();
                while (reader.Read())
                {
                    var member = new HomeMembership((Guid) reader["user"], (HomeMembershipAccess) (byte)reader["role"]);
                    members.Add(member);
                }

                return members;

            }, this.tokenSource.Token);
        }

        /// <summary>
        /// Add a home user
        /// </summary>
        /// <param name="homeMembership">The home membership definition</param>
        /// <param name="caller">The person doing the adding</param>
        /// <returns>The home membership objecdt</returns>
        public async Task<HomeMembership> AddHomeUser(HomeMembership homeMembership, Guid home, Guid caller)
        {
            await this.connectionManager.ExecuteSql("hub.addhomeuser", collection =>
            {
                collection.AddWithValue("home", home);
                collection.AddWithValue("caller", caller);
                collection.AddWithValue("user", homeMembership.User);
                collection.AddWithValue("role", (short)homeMembership.Access);
            }, this.tokenSource.Token);

            return homeMembership;
        }



        /// <summary>
        /// Get all devices for a home
        /// </summary>
        /// <param name="user">The user to get for</param>
        /// <param name="home">The home</param>
        /// <returns>The list of all devices attached to a home</returns>
        public async Task<IEnumerable<Device>> GetAllDevices(Guid user, Guid home)
        {
            return await this.connectionManager.ExecuteSql(
                "hub.getdevices",
                parameters =>
                    {
                        parameters.AddWithValue("home", home);
                        parameters.AddWithValue("user", user);
                    },
                reader =>
                    {
                        var functions = SqlDataLayer.GetDeviceFunctions(reader);

                        reader.NextResult();

                        var devices = new List<Device>();
                        while (reader.Read())
                        {
                            var deviceDefinitionGuid = (Guid)reader["devicedefinition"];

                            var definition = new DeviceDefinition(
                                deviceDefinitionGuid,
                                (string)reader["manufacturer"],
                                (string)reader["product"],
                                (DeviceType)reader["type"],
                                functions[deviceDefinitionGuid]);

                            var device = new Device(
                                (Guid)reader["id"],
                                home,
                                (string)reader["name"],
                                (string)reader["description"],
                                definition,
                                (string)reader["meta"]);
                            devices.Add(device);
                        }

                        return devices;
                    },
                this.tokenSource.Token);
        }

        /// <summary>
        /// The get device.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="device">The device.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Device> GetDevice(Guid user, Guid device)
        {
            return await this.connectionManager.ExecuteSql(
                "hub.getdevice",
                parameters =>
                    {
                        parameters.AddWithValue("user", user);
                        parameters.AddWithValue("device", device);
                    },
                reader =>
                    {
                        var functions = SqlDataLayer.GetDeviceFunctions(reader);

                        reader.NextResult();

                        reader.Read();

                        var deviceDefinitionGuid = (Guid)reader["devicedefinition"];

                        var definition = new DeviceDefinition(
                            deviceDefinitionGuid,
                            (string)reader["manufacturer"],
                            (string)reader["product"],
                            (DeviceType)reader["type"],
                            functions[deviceDefinitionGuid]);

                        return new Device(
                            (Guid)reader["id"],
                            (Guid)reader["home"],
                            (string)reader["name"],
                            (string)reader["description"],
                            definition,
                            (string)reader["meta"]);

                    },
                this.tokenSource.Token);
        }

        /// <summary>
        /// Get the functions for a device from a reader
        /// </summary>
        /// <param name="reader">The reader to use</param>
        /// <returns>The mapping of functions to device definitions</returns>
        private static Dictionary<Guid, List<DeviceFunction>> GetDeviceFunctions(IDataReader reader)
        {
            var functions = new Dictionary<Guid, List<DeviceFunction>>();

            // Read all of the device functions
            while (reader.Read())
            {
                var dev = (Guid)reader["device"];
                var func = new DeviceFunction((string)reader["name"]);

                if (false == functions.ContainsKey(dev))
                {
                    functions.Add(dev, new List<DeviceFunction>());
                }

                functions[dev].Add(func);
            }
            return functions;
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

        async Task<UserContext> IAccountLayer.AddAccount(Guid user, string manufacturer, Dictionary<string, string> loginMeta)
        {
            var usercontext = new UserContext(user, manufacturer, null);
            foreach (var keyval in loginMeta)
            {
                usercontext.AddLoginContext(keyval.Key, keyval.Value);
            }

            var doc = usercontext.SerializeLogin().ToString();

            await this.connectionManager.ExecuteSql(
                "hub.addaccountlogin",
                collection =>
                    {
                        collection.AddWithValue("user", user);
                        collection.AddWithValue("manufacturer", manufacturer);
                        collection.AddWithValue("meta", doc);
                    }, this.tokenSource.Token);

            return usercontext;
        }

        async Task<UserContext> IAccountLayer.GetAccount(Guid user, string manufacturer)
        {
            return await this.connectionManager.ExecuteSql(
                "hub.getaccountlogin",
                collection =>
                    {
                        collection.AddWithValue("user", user);
                        collection.AddWithValue("manufacturer", manufacturer);
                    },
                reader =>
                    {
                        reader.Read();
                        var meta = (string)reader["meta"];

                        var doc = XDocument.Parse(meta);
                        var usercontext = new UserContext(user, manufacturer, null);
                        usercontext.Load(doc);

                        return usercontext;
                    },
                this.tokenSource.Token);
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