namespace HomeHub.Data.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HomeHub.Common.Devices;
    using HomeHub.Data.Common.Models;

    /// <summary>
    /// The DataLayer interface.
    /// </summary>
    public interface IDataLayer
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
        /// Create a device
        /// </summary>
        /// <param name="device">The device which has been created</param>
        /// <returns>The created device</returns>
        Task<Device> CreateDevice(Device device);

        /// <summary>
        /// The create home.
        /// </summary>
        /// <param name="home">The home.</param>
        /// <param name="user">The user.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Home> CreateHome(Home home, Guid user);

        /// <summary>
        /// Gets the list of all homes accessible to a user
        /// </summary>
        /// <param name="user">The user id</param>
        /// <returns>The list of accessible homes</returns>
        Task<IEnumerable<Home>> GetHomes(Guid user);
    }
}
