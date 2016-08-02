namespace HomeHub.Service.Common.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HomeHub.Common;
    using HomeHub.Common.Devices;
    using HomeHub.Common.Devices.Light;
    using HomeHub.Common.Exceptions;
    using HomeHub.Service.Common.Models.Devices;
    using HomeHub.Service.Common.Models.Devices.Lights;

    /// <summary>
    /// The device data layer.
    /// </summary>
    public static class DeviceDataLayer
    {
        /// <summary>
        /// The definitions.
        /// </summary>
        private static Dictionary<string, IEnumerable<DeviceDefinition>> definitions = null;

        /// <summary>
        /// Get the device definitions. Using the cache.
        /// </summary>
        /// <returns>The device definitions</returns>
        public static async Task<Dictionary<string, IEnumerable<DeviceDefinition>>> GetDeviceDefinitions()
        {

            // If null, we need to retreive
            if (null == DeviceDataLayer.definitions)
            {
                // So get the definitions
                DeviceDataLayer.definitions = await DataLayer.Instance.GetDefinitions();
            }

            // Get the definitions
            return DeviceDataLayer.definitions;
        }

        /// <summary>
        /// Get all devices
        /// </summary>
        /// <param name="home">The home</param>
        /// <param name="user">The user</param>
        /// <returns>All of the devices for a home</returns>
        public static async Task<IEnumerable<DeviceModel>> GetDevices(Guid home, Guid user)
        {
            var accounts = await DataLayer.Accounts.GetAccount(user, home);

            var definitions = await DataLayer.Instance.GetDefinitions();

            var existing = new Dictionary<DeviceImport, Device>();
            try
            {
                var extem = await DataLayer.Instance.GetAllDevices(user, home);
                existing = extem.ToDictionary(e => new DeviceImport(e.Name, e.Definition.Product, e.Meta, null));
            }
            catch (ForbiddenDataAccessException e)
            {
                throw e;
                // nothing
            }
            catch (Exception)
            {
                // Nothin
            }

            var tasks = new List<Task<IEnumerable<DeviceModel>>>();
            foreach (var accountGroup in accounts.GroupBy(ac => ac.Manufacturer))
            {
                var manufacturer = accountGroup.Key;
                var adapter = DataLayer.AdapterManager.AdapterMap[manufacturer];
                var deviceDefinitions = definitions[accountGroup.Key];

                foreach (var account in accountGroup)
                {
                    tasks.Add(adapter.GetDevices(account).ContinueWith(
                        result =>
                            {
                                var devices = new List<DeviceModel>();
                                if (true == result.IsCompleted)
                                {
                                    var output = new Dictionary<Device, DeviceState>();
                                    foreach (var device in result.Result)
                                    {
                                        var def = deviceDefinitions.FirstOrDefault(d => d.Product.Equals(device.DeviceDescriptor));

                                        var match = existing.FirstOrDefault(e => adapter.Compare(e.Key, device));
                                        if (default(KeyValuePair<DeviceImport, Device>).Equals(match))
                                        {
                                            var tsk = DataLayer.Instance.CreateDevice(
                                                device.Name,
                                                home,
                                                string.Empty,
                                                def.Id,
                                                device.MetaData);
                                            tsk.Wait();
                                            output.Add(tsk.Result, device.DeviceState);
                                        }
                                        else
                                        {
                                            output.Add(match.Value, device.DeviceState);
                                        }
                                    }

                                    foreach (var device in output)
                                    {
                                        var model = new DeviceModel(device.Key, new LightStateModel((LightState)device.Value));
                                        devices.Add(model);
                                    }
                                }

                                return (IEnumerable<DeviceModel>)devices;
                            }));
                }
            }

            var results = await Task.WhenAll(tasks);

            // flatten and return
            return results.SelectMany(r => r);
        }
    }
}
