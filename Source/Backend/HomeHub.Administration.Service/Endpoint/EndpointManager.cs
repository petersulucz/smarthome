namespace HomeHub.Administration.Service.Endpoint
{
    using System;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.ServiceModel.Security;
    using System.Threading;

    using HomeHub.Administration.Common;
    using HomeHub.Administration.Common.Communication;

    using static HomeHub.Common.Trace.HomeHubEventSource;
    /// <summary>
    /// The endpoint manager.
    /// </summary>
    internal class EndpointManager
    {
        private readonly CancellationToken token;

        private readonly IPEndPoint endpoint;

        private ServiceHost serviceHost = null;

        public EndpointManager(CancellationToken token, int port)
        {
            this.token = token;
            this.endpoint = new IPEndPoint(IPAddress.Any, port);
        }

        public void Open()
        {
            var baseAddress = EndpointManager.GetServiceUri(this.endpoint.Port);


            var admin = new Uri(baseAddress, "Admin");
            //var mex = new Uri(GetHttpMexUri(this.endpoint.Port + 1), "mex");

            Log.Info($"Openning admin at: '{admin}'");
            var binding = BindingFactory.GetTcpBinding();
            this.serviceHost = new ServiceHost(typeof(HomeHubAdministrator), baseAddress);
            this.serviceHost.AddServiceEndpoint(
                typeof(IHomeHubAdministratorCom),
                binding,
                admin);

            this.serviceHost.Credentials.ServiceCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, "HomeHub-Admin-Server");
            this.serviceHost.Credentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, "HomeHub-Admin-Client");

            this.serviceHost.Open();
        }

        public void Close()
        {
            this.serviceHost?.Close();
        }

        private static Uri GetServiceUri(int port)
        {
            var hostname = Dns.GetHostName();
            return new Uri($"net.tcp://{hostname}:{port}/Admin");
        }

        private static Uri GetHttpMexUri(int port)
        {
            var hostname = Dns.GetHostName();
            return new Uri($"http://{hostname}:{port}/Admin");
        }
    }
}
