using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Data.Common.Models
{
    /// <summary>
    /// Represents a class which can be validated
    /// </summary>
    public interface IValidateable
    {
        /// <summary>
        /// Ensure that this object is valid
        /// </summary>
        void Validate();
    }
}
