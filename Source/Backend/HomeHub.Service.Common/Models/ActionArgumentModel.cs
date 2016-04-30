using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Models
{
    /// <summary>
    /// An action argument
    /// </summary>
    public class ActionArgumentModel
    {
        /// <summary>
        /// The name of the argument
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The value of the argument
        /// </summary>
        public string Value { get; set; }
    }
}
