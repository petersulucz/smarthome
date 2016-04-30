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
        public readonly static IDataLayer Instance = new SqlDataLayer();
    }
}
