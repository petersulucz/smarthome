namespace HomeHub.Service.Common.Models.Homes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using HomeHub.Data.Common.Models.Homes;

    /// <summary>
    /// Our idea of a home, which just contains basic information. Pretty much just a grouping object
    /// We could add more stuff to this... Like for example who has access to the home. But ill get to that later.
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
        /// The unique identifier for the home.
        /// This can be used to ... uniquely identify a home
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the home.
        /// </summary>
        [MaxLength(256)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The time that this home was created in UTC
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Convert to a common home.
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
