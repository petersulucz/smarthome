namespace HomeHub.Service.Common.Models.Devices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using HomeHub.Common.Devices;

    /// <summary>
    /// The device definition model.
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
        /// The functions that this device is capable of
        /// </summary>
        public IEnumerable<DeviceFunctionModel> Functions { get; set; }

        /// <summary>
        /// The type of this device
        /// </summary>
        public DeviceType Type { get; set; }

        /// <summary>
        /// The manufacturer of this device
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// The id of this defintion
        /// </summary>
        public Guid Id { get; set; }
    }
}
