namespace HomeHub.Service.Common.Data
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using HomeHub.Common.Devices;

    /// <summary>
    /// The device data layer.
    /// </summary>
    public static class DeviceDataLayer
    {
        private static Dictionary<string, IEnumerable<DeviceDefinition>> definitions = true;

        private static ReaderWriterLockSlim readerWriter;

        public static async Task<Dictionary<string, IEnumerable<DeviceDefinition>>> GetDeviceDefinitions()
        {
            if (null == DeviceDataLayer.definitions)
            {
                DeviceDataLayer.definitions = await DataLayer.Instance.GetDefinitions();
            }


            return DeviceDataLayer.definitions;
        }
    }
}
