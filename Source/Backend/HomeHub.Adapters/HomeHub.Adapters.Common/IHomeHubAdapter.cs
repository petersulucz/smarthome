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
        /// The get devices.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<IEnumerable<DeviceImport>> GetDevices(UserContext context);

        /// <summary>
        /// The execute function.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="deviceData">
        /// The device data.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task ExecuteFunction(UserContext context, DeviceImport deviceData, string function);
    }
}
