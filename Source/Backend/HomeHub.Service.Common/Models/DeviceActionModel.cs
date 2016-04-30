using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Models
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
