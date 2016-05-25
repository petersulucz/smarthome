using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Models.Security
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// An email password combo
    /// </summary>
    public sealed class UserPass
    {
        /// <summary>
        /// The email address
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The user password
        /// </summary>
        [Required]
        [MinLength(8, ErrorMessage = "Come on. Atleast 8 characters")]
        public string Password { get; set; }

        /// <summary>
        /// Get the security version of this object
        /// </summary>
        /// <returns></returns>
        public HomeHub.Data.Common.Models.Security.UserPass ToSecurityUserPass()
        {
            return new HomeHub.Data.Common.Models.Security.UserPass()
            {
                Email = this.Email,
                Password = this.Password,
            };
        }
    }
}
