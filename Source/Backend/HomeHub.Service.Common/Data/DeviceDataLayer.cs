namespace HomeHub.Service.Common.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HomeHub.Common;
    using HomeHub.Common.Devices;

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

    }
}
