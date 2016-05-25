using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Data.Common.Models.Security
{
    /// <summary>
    /// A user password combo
    /// </summary>
    public sealed class UserPass
    {
        /// <summary>
        /// The user email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The users password
        /// </summary>
        public string Password { get; set; }
    }
}
