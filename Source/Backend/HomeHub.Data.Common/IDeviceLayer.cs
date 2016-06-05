using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHub.Common.Devices;

namespace HomeHub.Data.Common
{
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
        /// Create a new device
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="home">The home</param>
        /// <param name="description">The description</param>
        /// <param name="definition">The definition</param>
        /// <returns>The new device</returns>
        Task<Device> CreateDevice(string name, Guid home, string description, Guid definition);
    }
}
