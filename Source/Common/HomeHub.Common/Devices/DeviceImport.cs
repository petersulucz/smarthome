namespace HomeHub.Common.Devices
{
    /// <summary>
    /// The device import.
    /// </summary>
    public class DeviceImport
    {
        public DeviceImport(string name, string deviceDescriptor, string metaData)
        {
            this.MetaData = metaData;
            this.DeviceDescriptor = deviceDescriptor;
            this.Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the device descriptor.
        /// </summary>
        public string DeviceDescriptor { get; private set; }

        /// <summary>
        /// Gets the meta data.
        /// </summary>
        public string MetaData { get; private set; }
    }
}
