using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    using HomeHub.Common.Devices;

    /// <summary>
    /// Represents a device
    /// </summary>
    [SuppressMessage("ReSharper", "StyleCop.SA1623")]
    public class DeviceModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceModel"/> class. 
        /// </summary>
        /// <param name="device">The device</param>
        public DeviceModel(Device device)
        {
            this.Name = device.Name;
            this.Home = device.Home;
            this.Description = device.Description;
            this.Definition = new DeviceDefinitionModel(device.Definition);
        }

        /// <summary>
        /// The definition of the device
        /// </summary>
        public DeviceDefinitionModel Definition { get; set; }

        /// <summary>
        /// The name of this device
        /// </summary>
        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The home this device references
        /// </summary>
        [Required]
        public Guid Home { get; set; }

        /// <summary>
        /// The description of this device
        /// </summary>
        [MaxLength(1024)]
        [Required]
        public string Description { get; set; }
    }
}
