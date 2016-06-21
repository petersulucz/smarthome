namespace HomeHub.Service.Common.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using HomeHub.Common.Devices;
    using HomeHub.Common.Exceptions;

    /// <summary>
    /// The device amanager.
    /// </summary>
    public static class DeviceManager
    {
        /// <summary>
        /// Execute an action on a device
        /// </summary>
        /// <param name="user">The user of the device</param>
        /// <param name="device">The device id</param>
        /// <param name="function">The function</param>
        /// <returns>Nothing</returns>
        public static async Task ExecuteAction(Guid user, Guid device, string function)
        {
            // Get the target device
            var target = await DataLayer.Instance.GetDevice(user, device);

            // Get the assosciated account
            var context = await DataLayer.Accounts.GetAccount(user, target.Home, target.Definition.Manufacturer);

            // Find the function
            var func = target.Definition.Functions.FirstOrDefault(f => string.Equals(f.Name, function, StringComparison.OrdinalIgnoreCase));

            if (null == func)
            {
                throw new NotFoundException("The function does not exist.");
            }

            // Get the adapter
            var adapter = DataLayer.AdapterManager.AdapterMap[target.Definition.Manufacturer];

            // Execute the function on the device
            await adapter.ExecuteFunction(
                context,
                new DeviceImport(target.Name, target.Definition.Product, target.Meta),
                func);
        }
    }
}
