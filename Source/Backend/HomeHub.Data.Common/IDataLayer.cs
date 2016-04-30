using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHub.Common.Devices;

namespace HomeHub.Data.Common
{
    public interface IDataLayer
    {
        Task<IEnumerable<Device>> GetAllDevices(Guid user);
    }
}
