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

        public static void Initialize(string connectionString)
        {
            Instance = new SqlDataLayer(connectionString);
            Security = new SqlDataLayer(connectionString);
        }
    }
}
