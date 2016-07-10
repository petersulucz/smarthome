namespace HomeHub.Common.Devices
{
    /// <summary>
    /// The device state.
    /// </summary>
    public abstract class DeviceState
    {
        protected DeviceState(DeviceType type, bool isConnected, bool isOn)
        {
            this.Type = type;
            this.IsConnected = isConnected;
            this.IsOn = isOn;
        }

        /// <summary>
        /// Gets a value indicating whether the device is on.
        /// </summary>
        public bool IsOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the device is connected.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public DeviceType Type
        {
            get; private set;
        }
    }
}
