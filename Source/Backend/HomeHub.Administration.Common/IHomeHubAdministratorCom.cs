namespace HomeHub.Administration.Common
{
    using System.ServiceModel;

    using HomeHub.Administration.Common.Models;

    /// <summary>
    /// The HomeHubAdministrator interface.
    /// </summary>
    [ServiceContract(Namespace = "http://homehub/admin")]
    public interface IHomeHubAdministratorCom
    {
        /// <summary>
        /// The test connection.
        /// </summary>
        /// <returns>
        /// The <see cref="HomeHubTestConnectionCom"/>.
        /// </returns>
        [OperationContract]
        HomeHubTestConnectionCom TestConnection();
    }
}
