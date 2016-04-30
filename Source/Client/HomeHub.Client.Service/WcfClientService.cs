using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HomeHub.Client.Service
{
    public class WcfClientService : IClientServiceContract
    {
        public string Ping()
        {
            return $"Im up {DateTime.UtcNow}";
        }
    }
}
