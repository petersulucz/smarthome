namespace HomeHub.Service.Common.Models.Devices
{
    using HomeHub.Common.Devices;

    /// <summary>
    /// The model which shows the device state
    /// </summary>
    public abstract class DeviceStateModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceStateModel"/> class. 
        /// </summary>
        /// <param name="state">
        /// </param>
        protected DeviceStateModel(DeviceState state)
        {
            this.IsConnected = state.IsConnected;
            this.IsOn = state.IsOn;
            this.Type = state.Type;
        }

        /// <summary>
        /// Gets the type of the device. Whether it is a light or whatever
        /// </summary>
        public DeviceType Type { get; private set; }

        /// <summary>
        /// Gets whether the device can currently be reached
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Gets whether the device is powered
        /// </summary>
        public bool IsOn { get; private set; }
    }
}
