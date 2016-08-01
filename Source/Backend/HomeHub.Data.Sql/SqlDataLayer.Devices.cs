namespace HomeHub.Data.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HomeHub.Common.Devices;
    using HomeHub.Data.Common;

    /// <summary>
    /// The sql data layer.
    /// </summary>
    public partial class SqlDataLayer : IDeviceLayer
    {
        /// <summary>
        /// Create a new device
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="home">The home</param>
        /// <param name="description">The description</param>
        /// <param name="definition">The definition</param>
        /// <param name="meta">The metadata about the device</param>
        /// <returns>The new device</returns>
        async Task<Device> IDeviceLayer.CreateDevice(string name, Guid home, string description, Guid definition, string meta)
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
        /// The get device.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="device">The device.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        async Task<Device> IDeviceLayer.GetDevice(Guid user, Guid device)
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
        /// Get all devices for a home
        /// </summary>
        /// <param name="user">The user to get for</param>
        /// <param name="home">The home</param>
        /// <returns>The list of all devices attached to a home</returns>
        async Task<IEnumerable<Device>> IDeviceLayer.GetAllDevices(Guid user, Guid home)
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
    }
}
