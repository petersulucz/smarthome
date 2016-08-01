namespace HomeHub.Data.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    using HomeHub.Common.Devices;
    using HomeHub.Data.Common;

    /// <summary>
    /// The SQL data layer.
    /// </summary>
    public partial class SqlDataLayer : IDeviceDefinitions
    {

        /// <summary>
        /// Get the device definitions
        /// </summary>
        /// <returns>The dictionary of device definitions</returns>
        async Task<Dictionary<string, IEnumerable<DeviceDefinition>>> IDeviceDefinitions.GetDefinitions()
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
    }
}
