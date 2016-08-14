namespace HomeHub.Administration.Service
{
    using System;
    using System.ServiceModel;

    using HomeHub.Administration.Common;
    using HomeHub.Administration.Common.Models;

    /// <summary>
    /// The home hub administrator.
    /// </summary>
    internal class HomeHubAdministrator : IHomeHubAdministratorCom
    {
        /// <summary>
        /// The test connection.
        /// </summary>
        /// <returns>
        /// The <see cref="HomeHubTestConnectionCom"/>.
        /// </returns>
        public HomeHubTestConnectionCom TestConnection()
        {
            return new HomeHubTestConnectionCom("hi");
        }
    }
}
