using System.Collections.Generic;
using System.Threading.Tasks;
using HomeHub.Common.Devices;

namespace HomeHub.Data.Common
{
    public interface IDeviceDefinitions
    {
        /// <summary>
        /// Get all device definitions
        /// </summary>
        /// <returns>The device definitions</returns>
        Task<Dictionary<string, IEnumerable<DeviceDefinition>>> GetDefinitions();
    }
}
