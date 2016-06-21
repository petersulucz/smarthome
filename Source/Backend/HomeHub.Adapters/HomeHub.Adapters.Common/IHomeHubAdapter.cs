namespace HomeHub.Adapters.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HomeHub.Common.Devices;

    /// <summary>
    /// The HomeHubAdapter interface.
    /// </summary>
    public interface IHomeHubAdapter
    {
        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        /// Check if two devices are the same devices within griddle
        /// </summary>
        /// <param name="a">The a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// True if the devices are the same physical device. False otherwise.
        /// </returns>
        bool Compare(DeviceImport a, DeviceImport b);

        /// <summary>
        /// The get devices.
        /// </summary>
        /// <param name="context">The user context.</param>
        /// <returns>
        /// A task.
        /// </returns>
        Task<IEnumerable<DeviceImport>> GetDevices(UserContext context);

        /// <summary>
        /// Execute a function on a device
        /// </summary>
        /// <param name="context">The user context.</param>
        /// <param name="deviceData">The device data.</param>
        /// <param name="function">The function to execute.</param>
        /// <returns>
        /// A task.
        /// </returns>
        Task ExecuteFunction(UserContext context, DeviceImport deviceData, DeviceFunction function);
    }
}
