namespace HomeHub.Common.Devices
{
    /// <summary>
    /// The device import.
    /// </summary>
    public class DeviceImport
    {
        public DeviceImport(string name, string deviceDescriptor, string metaData, DeviceState state)
        {
            this.MetaData = metaData;
            this.DeviceDescriptor = deviceDescriptor;
            this.Name = name;
            this.DeviceState = state;
        }

        /// <summary>
        /// Gets the device state.
        /// </summary>
        public DeviceState DeviceState { get; private set; }

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
