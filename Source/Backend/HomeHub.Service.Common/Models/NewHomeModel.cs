namespace HomeHub.Service.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using HomeHub.Data.Common.Models;

    /// <summary>
    /// The new home model.
    /// </summary>
    public class NewHomeModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        [MaxLength(256)]
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
