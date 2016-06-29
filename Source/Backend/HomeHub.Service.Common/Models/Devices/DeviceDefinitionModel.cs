namespace HomeHub.Service.Common.Models.Devices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using HomeHub.Common.Devices;

    /// <summary>
    /// This is the model for the physical device. It is what we see the device as, and what the supported function are.
    /// </summary>
    public class DeviceDefinitionModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDefinitionModel"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        public DeviceDefinitionModel(DeviceDefinition definition)
        {
            this.Id = definition.Id;
            this.Manufacturer = definition.Manufacturer;
            this.Type = definition.Type;
            this.Functions = definition.Functions.Select(function => new DeviceFunctionModel(function));
        }

        /// <summary>
        /// The functions that this device is capable of performing. This may not be all of the functions. But its all the functions we support so deal with it.
        /// </summary>
        public IEnumerable<DeviceFunctionModel> Functions { get; set; }

        /// <summary>
        /// The type of this device. This is a string. LIGHT, TV whatever
        /// </summary>
        public DeviceType Type { get; set; }

        /// <summary>
        /// The manufacturer name of this device
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// The unique identifier for this definition
        /// </summary>
        public Guid Id { get; set; }
    }
}
