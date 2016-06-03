namespace HomeHub.Common.Devices
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The device definition.
    /// </summary>
    public class DeviceDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDefinition"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="type">The type.</param>
        /// <param name="functions">The functions</param>
        public DeviceDefinition(Guid id, string manufacturer, DeviceType type, IEnumerable<DeviceFunction> functions)
        {
            this.Id = id;
            this.Manufacturer = manufacturer;
            this.Type = type;
            this.Functions = new List<DeviceFunction>(functions);
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        public string Manufacturer { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public DeviceType Type { get; private set; }

        /// <summary>
        /// Gets the functions.
        /// </summary>
        public IEnumerable<DeviceFunction> Functions { get; private set; }
    }
}
