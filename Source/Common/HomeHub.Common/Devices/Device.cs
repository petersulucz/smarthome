using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Common.Devices
{
    public abstract class Device : IDevice
    {
        public Device(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        internal Guid Id { get; private set; }
        public string Name { get; private set; }
        public abstract string DeviceType { get; }
        public abstract Task Off();
        public abstract Task On();
    }
}
