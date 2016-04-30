using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Configuration;
using System.Configuration.Install;
using System.ComponentModel;
using System.ServiceProcess;

namespace HomeHub.Client.ServiceHost
{
    [RunInstaller(true)]
    class ClientServiceInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ClientServiceInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
           
            service.ServiceName = "HomeHubClient";
            service.Description = "The home hub client service for upnp control.";
            service.StartType = ServiceStartMode.Automatic;
            
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
