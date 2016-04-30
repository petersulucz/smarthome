using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Common.Devices
{
    public interface IDevice
    {
        string Name { get; }
        Task On();
        Task Off();
    }
}
