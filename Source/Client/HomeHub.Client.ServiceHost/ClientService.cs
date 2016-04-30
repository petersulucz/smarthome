using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Client.ServiceHost
{
    using Service;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using ServiceHost = System.ServiceModel.ServiceHost;
    public partial class ClientService : ServiceBase
    {
        ServiceHost host;

        public ClientService()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.host = new ServiceHost(typeof(WcfClientService), new Uri("net.tcp://localhost:9811"));

            this.CreateMainEndpoints();

            this.host.Open();
        }

        protected override void OnStart(string[] args)
        {
            Init();
        }

        protected override void OnStop()
        {
            if(null != host)
            {
                host.Close();
            }
        }

        private void CreateMainEndpoints()
        {
            var binding = new NetTcpBinding(SecurityMode.None);

            var mexBehavior = new ServiceMetadataBehavior();
            this.host.Description.Behaviors.Add(mexBehavior);

            this.host.AddServiceEndpoint(typeof(IClientServiceContract), binding, "");

            this.host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName,
                                MetadataExchangeBindings.CreateMexTcpBinding(),
                                "mex");
        }
    }
}
