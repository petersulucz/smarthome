namespace HomeHub.Service.Common.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HomeHub.Common.Devices;

    using static HomeHub.Common.Trace.HomeHubEventSource;

    /// <summary>
    /// The account manager.
    /// </summary>
    public static class AccountManager
    {
        /// <summary>
        /// Link an account
        /// </summary>
        /// <param name="user">The user id</param>
        /// <param name="home">The home.</param>
        /// <param name="manufacturer">The manufacturer</param>
        /// <param name="loginMeta">The login meta data</param>
        /// <returns>
        /// On completion
        /// </returns>
        public static async Task<IEnumerable<Device>> LinkAccount(Guid user, Guid home, string manufacturer, Dictionary<string, string> loginMeta)
        {
            Log.MethodEnter();

            var adapter = DataLayer.AdapterManager.AdapterMap[manufacturer];

            Log.Verbose("Getting user context.");
            var context = await DataLayer.Accounts.AddAccount(user, home, manufacturer, loginMeta);

            Log.Verbose("Getting devices for user.");
            var devices = await adapter.GetDevices(context);

            Log.Verbose("Get device definitions.");
            var definitionsDict = await DeviceDataLayer.GetDeviceDefinitions();
            var definitions = definitionsDict[manufacturer];

            Log.Verbose("Getting existing devices.");
            var existing = Enumerable.Empty<DeviceImport>();
            try
            {
                var extem = await DataLayer.Instance.GetAllDevices(user, home);
                existing = extem.Select(e => new DeviceImport(e.Name, e.Definition.Product, e.Meta, null));
            }
            catch
            { 
                // nothing
            }

            var output = new List<Device>();
            foreach (var device in devices)
            {
                if (true == existing.Any(e => adapter.Compare(e, device)))
                {
                    continue;
                }

                var def = definitions.FirstOrDefault(d => d.Product.Equals(device.DeviceDescriptor));

                if (null != def)
                {
                    output.Add(await DataLayer.Instance.CreateDevice(device.Name, home, string.Empty, def.Id, device.MetaData));
                }
            }

            Log.MethodLeave();
            return output;
        }
    }
}
