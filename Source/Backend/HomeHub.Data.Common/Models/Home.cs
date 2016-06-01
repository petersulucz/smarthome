namespace HomeHub.Data.Common.Models
{
    using System;

    /// <summary>
    /// The home.
    /// </summary>
    public class Home
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class. 
        /// </summary>
        /// <param name="name">The name of the home</param>
        /// <param name="created">The created date</param>
        /// <param name="id">The id of the home</param>
        public Home(string name, DateTime created, Guid id)
        {
            this.Name = name;
            this.Created = created;
            this.Id = id;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the created date.
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; private set; }
    }
}
