using System.Collections.Generic;

namespace HomeHub.Service.Common.Models.Devices
{
    /// <summary>
    /// The device action model.
    /// </summary>
    public class DeviceActionModel
    {
        /// <summary>
        /// The name of the supported action.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of arguments for an action
        /// </summary>
        public IEnumerable<ActionArgumentModel> Arguments { get; set; }
    }
}
