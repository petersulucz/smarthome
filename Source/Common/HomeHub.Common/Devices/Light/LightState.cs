namespace HomeHub.Common.Devices.Light
{
    /// <summary>
    /// The light state.
    /// </summary>
    public class LightState : DeviceState
    {
        public LightState(bool isConnected, bool isOn, int color)
            : base(DeviceType.Light, isConnected, isOn)
        {
            this.Color = color;
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        public int Color { get; private set; }

    }
}
