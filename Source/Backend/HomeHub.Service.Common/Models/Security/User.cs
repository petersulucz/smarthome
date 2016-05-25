using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Models.Security
{
    /// <summary>
    /// Creates a new user
    /// </summary>
    public class User
    {
        /// <summary>
        /// The first name
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; }

        /// <summary>
        /// User last name
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string LastName { get; set; }

        /// <summary>
        /// The email and password
        /// </summary>
        [Required]
        public UserPass LoginInfo { get; set; }
        /// <summary>
        /// To a security user
        /// </summary>
        /// <returns></returns>
        public HomeHub.Data.Common.Models.Security.CreateUser ToSecurityUser()
        {
            var sec = new HomeHub.Data.Common.Models.Security.CreateUser()
                       {
                           FirstName = this.FirstName,
                           LastName = this.LastName,
                           LoginInfo = this.LoginInfo.ToSecurityUserPass()
                       };
            sec.Validate();
            return sec;
        }
    }
}
