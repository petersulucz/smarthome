using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Data
{
    using HomeHub.Common.Devices;

    public static class DeviceManager
    {
        public static async Task ExecuteAction(Guid user, Guid device, string function)
        {
            var target = await DataLayer.Instance.GetDevice(user, device);
            var context = await DataLayer.Accounts.GetAccount(user, target.Definition.Manufacturer);
            var adapter = DataLayer.AdapterManager.AdapterMap[target.Definition.Manufacturer];
            await adapter.ExecuteFunction(
                context,
                new DeviceImport(target.Name, target.Definition.Product, target.Meta),
                function);
        }
    }
}
