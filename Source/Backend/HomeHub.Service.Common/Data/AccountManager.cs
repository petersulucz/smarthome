using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Service.Common.Data
{
    using HomeHub.Common.Devices;

    public static class AccountManager
    {
        /// <summary>
        /// Link an account
        /// </summary>
        /// <param name="user">The user id</param>
        /// <param name="manufacturer">The manufacturer</param>
        /// <param name="loginMeta">The login meta data</param>
        /// <returns>On completion</returns>
        public static async Task<IEnumerable<Device>>  LinkAccount(Guid user, Guid home, string manufacturer, Dictionary<string, string> loginMeta)
        {
            var context = await DataLayer.Accounts.AddAccount(user, manufacturer, loginMeta);
            var devices = await DataLayer.AdapterManager.AdapterMap[manufacturer].GetDevices(context);

            var definitionsDict = await DataLayer.Instance.GetDefinitions();
            var definitions = definitionsDict[manufacturer];

            var output = new List<Device>();
            foreach (var device in devices)
            {
                var def = definitions.FirstOrDefault(d => d.Product.Equals(device.DeviceDescriptor));

                if (null != def)
                {
                    output.Add(await DataLayer.Instance.CreateDevice(device.Name, home, String.Empty, def.Id, device.MetaData));
                }
            }

            return output;
        }
    }
}
