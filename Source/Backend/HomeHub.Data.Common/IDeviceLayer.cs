namespace HomeHub.Data.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HomeHub.Common.Devices;

    /// <summary>
    /// The DeviceLayer interface.
    /// </summary>
    public interface IDeviceLayer
    {
        /// <summary>
        /// Get all devices for a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="home">The home to search.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<IEnumerable<Device>> GetAllDevices(Guid user, Guid home);

        /// <summary>
        /// The get device.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="device">The device.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Device> GetDevice(Guid user, Guid device);

        /// <summary>
        /// Create a new device
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="home">The home</param>
        /// <param name="description">The description</param>
        /// <param name="definition">The definition</param>
        /// <param name="metadata">The device metadata</param>
        /// <returns>The new device</returns>
        Task<Device> CreateDevice(string name, Guid home, string description, Guid definition, string metadata);
    }
}
