using HomeHub.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHub.Common.Devices;

namespace HomeHub.Data.Sql
{
    public class SqlDataLayer : IDataLayer
    {
        Task<IEnumerable<Device>> IDataLayer.GetAllDevices(Guid user)
        {

            var lst = new[]
                    {
                        new Light(Guid.Empty, "device 1"),
                        new Light(Guid.Empty, "device 2")
                    };
            return Task.FromResult((IEnumerable<Device>)(lst));
        }
    }
}
