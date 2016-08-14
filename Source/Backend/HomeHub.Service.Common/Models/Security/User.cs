using System.ComponentModel.DataAnnotations;

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
        [MaxLength(64, ErrorMessage = "First Name SHALL BE SHORTER THAN 64 characters")]
        [MinLength(2, ErrorMessage = "Your name is longer than 1 character.")]
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
