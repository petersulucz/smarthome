namespace HomeHub.Service.Common.Models.Devices
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    using HomeHub.Common.Devices;

    /// <summary>
    /// This models a device in terms of of smart home. This contains information, along with all of the functions which are supported.
    /// Also the actual product definition for this device are actually included in this thing.
    /// </summary>
    [SuppressMessage("ReSharper", "StyleCop.SA1623")]
    public sealed class DeviceModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceModel"/> class. 
        /// </summary>
        /// <param name="device">The device</param>
        /// <param name="state">The current state of the device</param>
        public DeviceModel(Device device, DeviceStateModel state)
        {
            this.Id = device.Id;
            this.Name = device.Name;
            this.Home = device.Home;
            this.Description = device.Description;
            this.Definition = new DeviceDefinitionModel(device.Definition);
            this.State = state;
        }

        /// <summary>
        /// The unique identifier for this device.
        /// This is just a guid. It has nothing to do with the product.
        /// It is just a way for us to address this thing.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of this device. Need I say more. This is the name you give the device.
        /// </summary>
        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// THe unique identifier for the home which this device belongs too.
        /// </summary>
        [Required]
        public Guid Home { get; set; }

        /// <summary>
        /// The string description for this device. For now this field is always blank.
        /// </summary>
        [MaxLength(1024)]
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// The definition model for this device.
        /// This contains information about what this device is and what functions it supports.
        /// </summary>
        public DeviceDefinitionModel Definition { get; set; }

        /// <summary>
        /// The current state of the device
        /// </summary>
        public DeviceStateModel State { get; set; }
    }
}
