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

        /// <summary>
        /// Create a new device
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="home">The home</param>
        /// <param name="description">The description</param>
        /// <param name="definition">The definition</param>
        /// <returns>The new device</returns>
        Task<Device> CreateDevice(string name, Guid home, string description, Guid definition);

        /// <summary>
        /// Get all device definitions
        /// </summary>
        /// <returns>The device definitions</returns>
        Task<Dictionary<string, IEnumerable<DeviceDefinition>>> GetDefinitions();

    }
}
