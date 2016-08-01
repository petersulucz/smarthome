namespace HomeHub.Data.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HomeHub.Data.Common;
    using HomeHub.Data.Common.Models.Homes;

    /// <summary>
    /// The sql data layer.
    /// </summary>
    public partial class SqlDataLayer : IHomeLayer
    {
        /// <summary>
        /// Create a new home
        /// </summary>
        /// <param name="home">The home to create</param>
        /// <param name="user">The user creating the home</param>
        /// <returns></returns>
        async Task<Home> IHomeLayer.CreateHome(Home home, Guid user)
        {
            return await this.connectionManager.ExecuteSql(
                "hub.createhome",
                collection =>
                    {
                        collection.AddWithValue("name", home.Name);
                        collection.AddWithValue("user", user);
                    },
                reader =>
                    {
                        reader.Read();
                        return SqlDataLayer.ReadHome(reader);
                    },
                this.tokenSource.Token);
        }

        /// <summary>
        /// Get the homes for a user
        /// </summary>
        /// <param name="user">The user id</param>
        /// <returns>A list of all homes for this user</returns>
        async Task<IEnumerable<Home>> IHomeLayer.GetHomes(Guid user)
        {
            return await this.connectionManager.ExecuteSql(
                "hub.gethomes",
                collection =>
                    {
                        collection.AddWithValue("user", user);
                    },
                reader =>
                    {
                        var result = new List<Home>();
                        while (reader.Read())
                        {
                            result.Add(SqlDataLayer.ReadHome(reader));
                        }
                        return result;
                    },
                this.tokenSource.Token);
        }

        /// <summary>
        /// Get all of the users attached to a house
        /// </summary>
        /// <param name="home">The home to check</param>
        /// <param name="user">The user calling the method</param>
        /// <returns>The list of home memberships</returns>
        async Task<IEnumerable<HomeMembership>> IHomeLayer.GetHomeUsers(Guid home, Guid user)
        {
            return await this.connectionManager.ExecuteSql(
                "hub.gethomeusers",
                collection =>
                    {
                        collection.AddWithValue("home", home);
                        collection.AddWithValue("user", user);
                    },
                reader =>
                    {

                        var members = new List<HomeMembership>();
                        while (reader.Read())
                        {
                            var member = new HomeMembership(
                                (Guid)reader["user"],
                                (HomeMembershipAccess)(byte)reader["role"]);
                            members.Add(member);
                        }

                        return members;

                    },
                this.tokenSource.Token);
        }

        /// <summary>
        /// Add a home user
        /// </summary>
        /// <param name="homeMembership">The home membership definition</param>
        /// <param name="caller">The person doing the adding</param>
        /// <returns>The home membership objecdt</returns>
        async Task<HomeMembership> IHomeLayer.AddHomeUser(HomeMembership homeMembership, Guid home, Guid caller)
        {
            await this.connectionManager.ExecuteSql(
                "hub.addhomeuser",
                collection =>
                    {
                        collection.AddWithValue("home", home);
                        collection.AddWithValue("caller", caller);
                        collection.AddWithValue("user", homeMembership.User);
                        collection.AddWithValue("role", (short)homeMembership.Access);
                    },
                this.tokenSource.Token);

            return homeMembership;
        }


    }
}
