using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHub.Data.Common.Models;
using HomeHub.Data.Common.Models.Homes;

namespace HomeHub.Data.Common
{
    public interface IHomeLayer
    {
        /// <summary>
        /// The create home.
        /// </summary>
        /// <param name="home">The home.</param>
        /// <param name="user">The user.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Home> CreateHome(Home home, Guid user);

        /// <summary>
        /// Gets the list of all homes accessible to a user
        /// </summary>
        /// <param name="user">The user id</param>
        /// <returns>The list of accessible homes</returns>
        Task<IEnumerable<Home>> GetHomes(Guid user);

        /// <summary>
        /// Get all of the users attached to a house
        /// </summary>
        /// <param name="home">The home to check</param>
        /// <param name="user">The user calling the method</param>
        /// <returns>The list of home memberships</returns>
        Task<IEnumerable<HomeMembership>> GetHomeUsers(Guid home, Guid user);

        /// <summary>
        /// Add a home user
        /// </summary>
        /// <param name="homeMembership">The home membership definition</param>
        /// <param name="home">The home id</param>
        /// <param name="caller">The person doing the adding</param>
        /// <returns>The home membership objecdt</returns>
        Task<HomeMembership> AddHomeUser(HomeMembership homeMembership, Guid home, Guid caller);
    }
}
