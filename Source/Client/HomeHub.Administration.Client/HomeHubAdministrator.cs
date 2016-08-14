namespace HomeHub.Administration.Client
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
    using System.ServiceModel.Security;

    using HomeHub.Administration.Common;
    using HomeHub.Administration.Common.Communication;

    /// <summary>
    /// The home hub administrator.
    /// </summary>
    public class HomeHubAdministrator
    {
        private readonly string endpointAddress = null;

        private readonly ChannelFactory<IHomeHubAdministratorCom> channelFactory;

        public HomeHubAdministrator(string endpoint)
        {
            this.endpointAddress = $"net.tcp://{endpoint}:8001/Admin";
            this.channelFactory = new ChannelFactory<IHomeHubAdministratorCom>(
                BindingFactory.GetTcpBinding(),
                new EndpointAddress(new Uri(this.endpointAddress)));

            this.channelFactory.Credentials.ClientCertificate.SetCertificate(
                StoreLocation.CurrentUser,
                StoreName.My,
                X509FindType.FindBySubjectName,
                "HomeHub-Admin-Client");
            this.channelFactory.Credentials.ServiceCertificate.SetScopedCertificate(
                StoreLocation.CurrentUser,
                StoreName.My,
                X509FindType.FindBySubjectName,
                "HomeHub-Admin-Server",
                new Uri(this.endpointAddress));

        }

        public bool TestConnection()
        {
            var result = this.InvokeAdministrator(channel => 
            channel.TestConnection());
            return true;
        }

        private T InvokeAdministrator<T>(Func<IHomeHubAdministratorCom, T> action)
        {
            return action(this.GetChannel());
        }

        private IHomeHubAdministratorCom GetChannel()
        {
            return this.channelFactory.CreateChannel();
        }
    }
}
