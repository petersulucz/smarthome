using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The new device model.
    /// </summary>
    public class NewDeviceModel
    {
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

        /// <summary>
        /// The device definition
        /// </summary>
        [Required]
        public Guid DeviceDefinition { get; set; }
    }
}
