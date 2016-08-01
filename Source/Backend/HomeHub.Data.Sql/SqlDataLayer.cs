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
    using HomeHub.Data.Common.Models.Security;
    using HomeHub.Data.Common.Security;

    public partial class SqlDataLayer : IDataLayer, IDisposable
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
                var func = new DeviceFunction((string)reader["name"], (FunctionArgumentType)(int)reader["argumenttype"]);

                if (false == functions.ContainsKey(dev))
                {
                    functions.Add(dev, new List<DeviceFunction>());
                }

                functions[dev].Add(func);
            }
            return functions;
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