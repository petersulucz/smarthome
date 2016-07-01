namespace HomeHub.Service.Common.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HomeHub.Service.Common.Models.Homes;

    /// <summary>
    /// The home manager
    /// </summary>
    public sealed class HomeManager
    {
        /// <summary>
        /// Get homes for a user
        /// </summary>
        /// <param name="user">The id of the user</param>
        /// <param name="name">The name of the home</param>
        /// <returns>The task which gets the homes</returns>
        public static Task<IEnumerable<HomeModel>> GetHomes(Guid user, string name)
        {
            return DataLayer.Instance.GetHomes(user).ContinueWith(
                result =>
                    {
                        var homes = result.Result;
                        if (false == string.IsNullOrWhiteSpace(name))
                        {
                            homes = homes.Where(h => h.Name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) >= 0);
                        }

                        return homes.Select(h => new HomeModel(h));
                    });
        }
    }
}
