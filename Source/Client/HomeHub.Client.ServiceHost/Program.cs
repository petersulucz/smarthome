using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHub.Client.ServiceHost
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                ServiceBase.Run(new ClientService());
            }
            else
            {
                var svc = new ClientService();
                svc.Init();
            }
        }
    }
}
