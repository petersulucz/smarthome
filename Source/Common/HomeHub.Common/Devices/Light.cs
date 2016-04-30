using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Common.Devices
{
    public class Light : Device, ILight
    {
        public Light(Guid id, string name) : base(id, name)
        {

        }

        public override string DeviceType
        {
            get
            {
                return "LIGHT";
            }
        }

        public override Task Off()
        {
            throw new NotImplementedException();
        }

        public override Task On()
        {
            throw new NotImplementedException();
        }
    }
}
