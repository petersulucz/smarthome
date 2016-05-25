using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Data.Common.Models.Security
{
    using HomeHub.Common.Exceptions;

    /// <summary>
    /// A user security model
    /// </summary>
    public sealed class CreateUser : IValidateable
    {
        /// <summary>
        /// The user firstname
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The login information
        /// </summary>
        public UserPass LoginInfo { get; set; }

        public void Validate()
        {
            if (true == string.IsNullOrWhiteSpace(this.FirstName))
            {
                ExceptionUtility.ThrowInvalidDataException("Firstname cannot be blank");
            }

            if (true == string.IsNullOrWhiteSpace(this.LastName))
            {
                ExceptionUtility.ThrowInvalidDataException("Lastname cannot be blank");
            }

            if (true == string.IsNullOrWhiteSpace(this.LoginInfo.Email))
            {
                ExceptionUtility.ThrowInvalidDataException("Email cannot be blank");
            }
        }
    }
}
