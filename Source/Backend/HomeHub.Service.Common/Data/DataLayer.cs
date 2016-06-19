using HomeHub.Data.Common;
using HomeHub.Data.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Data
{
    public static class DataLayer
    {
        public static IDataLayer Instance { get; private set; }

        public static ISecurityLayer Security { get; private set; }

        public static IAccountLayer Accounts { get; private set; }

        public static AdapterManager AdapterManager { get; private set; }

        public static void Initialize(string connectionString, string adapterFolder)
        {
            Instance = new SqlDataLayer(connectionString);
            Security = new SqlDataLayer(connectionString);
            Accounts = new SqlDataLayer(connectionString);
            AdapterManager = new AdapterManager(adapterFolder);
        }
    }
}
