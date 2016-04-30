using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Models
{
    /// <summary>
    /// Represents a device
    /// </summary>
    public class DeviceModel
    {
        /// <summary>
        /// The name of the device
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the device
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// The list of supported actions on a device
        /// </summary>
        public IEnumerable<DeviceActionModel> Actions { get; set; }
    }
}
