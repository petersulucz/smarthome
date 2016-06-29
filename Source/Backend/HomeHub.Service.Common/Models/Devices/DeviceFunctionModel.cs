namespace HomeHub.Service.Common.Models.Devices
{
    using HomeHub.Common.Devices;

    /// <summary>
    /// The device funtion model. This represents all of the functions which a device is capable of performing.
    /// </summary>
    public class DeviceFunctionModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceFunctionModel"/> class.
        /// </summary>
        /// <param name="function">The function.</param>
        public DeviceFunctionModel(DeviceFunction function)
        {
            this.Name = function.Name;
            this.ArgumentType = function.ArgumentType.ToString();
        }

        /// <summary>
        /// The name of this function
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The argument type of the function. This is the actual value type that this takes. STRING, INT, DOUBLE
        /// </summary>
        public string ArgumentType { get; set; }
    }
}
