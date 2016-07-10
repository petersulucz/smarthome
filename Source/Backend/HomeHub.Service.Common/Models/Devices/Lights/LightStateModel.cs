namespace HomeHub.Service.Common.Models.Devices.Lights
{
    using HomeHub.Common.Devices;
    using HomeHub.Common.Devices.Light;

    /// <summary>
    /// The light state model.
    /// </summary>
    public class LightStateModel : DeviceStateModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightStateModel"/> class. 
        /// </summary>
        /// <param name="state">
        /// The current state of the device
        /// </param>
        public LightStateModel(LightState state)
            : base(state)
        {
            this.Color = state.Color;
        }

        /// <summary>
        /// Gets the current color of the light
        /// </summary>
        public int Color { get; private set; }
    }
}
