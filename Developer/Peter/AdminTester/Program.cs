using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminTester
{
    using System.Net;

    using HomeHub.Administration.Client;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var admin = new HomeHubAdministrator(Dns.GetHostName());
            admin.TestConnection();
        }
    }
}
