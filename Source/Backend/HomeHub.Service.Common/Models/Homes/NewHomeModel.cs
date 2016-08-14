using System;
using System.ComponentModel.DataAnnotations;
using HomeHub.Data.Common.Models.Homes;

namespace HomeHub.Service.Common.Models.Homes
{
    /// <summary>
    /// The new home model.
    /// </summary>
    public class NewHomeModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        [MaxLength(256, ErrorMessage = "Home name SHALL NOT be longer than 256 characters.")]
        [MinLength(4, ErrorMessage = "Home name SHALL be longer than 4 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Converts to a common home
        /// </summary>
        /// <returns>
        /// The <see cref="Home"/>.
        /// </returns>
        public Home ToHome()
        {
            return new Home(this.Name, DateTime.Now, Guid.Empty);
        }
    }
}
