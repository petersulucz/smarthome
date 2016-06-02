namespace HomeHub.Service.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using HomeHub.Data.Common.Models;

    /// <summary>
    /// The home model.
    /// </summary>
    public class HomeModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeModel"/> class.
        /// </summary>
        /// <param name="home">
        /// The home.
        /// </param>
        public HomeModel(Home home)
        {
            this.Name = home.Name;
            this.Created = home.Created;
            this.Id = home.Id;
        }

        /// <summary>
        /// Gets or sets the id of the home
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [MaxLength(256)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the created date time in UTC.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Conver to a common home.
        /// </summary>
        /// <returns>
        /// The <see cref="Home"/>.
        /// </returns>
        public Home ToHome()
        {
            return new Home(this.Name, this.Created, this.Id);
        }
    }
}
