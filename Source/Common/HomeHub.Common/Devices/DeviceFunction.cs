namespace HomeHub.Common.Devices
{
    using System.Collections.Generic;

    /// <summary>
    /// The device function.
    /// </summary>
    public class DeviceFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceFunction"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DeviceFunction(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }
    }
}
