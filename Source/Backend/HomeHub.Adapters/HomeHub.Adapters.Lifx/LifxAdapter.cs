using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Adapters.Lifx
{
    using System.ComponentModel.Composition;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;

    using HomeHub.Adapters.Common;
    using HomeHub.Common.Devices;

    using Newtonsoft.Json.Linq;

    [Export(typeof(IHomeHubAdapter))]
    public class LifxAdapter : IHomeHubAdapter
    {
        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        string IHomeHubAdapter.Manufacturer => "lifx";

        /// <summary>
        /// The get devices.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IEnumerable<DeviceImport>> GetDevices(UserContext context)
        {
            var token = context.GetLogin("appkey");
            var str = await LifxAdapter.Get(new Uri("https://api.lifx.com/v1/lights/all"), token, CancellationToken.None);

            var lightJsonArray = JArray.Parse(str);

            var lights = new List<DeviceImport>();

            foreach (var lightJson in lightJsonArray)
            {
                var name = lightJson["label"].ToString();

                var descriptor = lightJson["product"]["identifier"].ToString();

                var meta = new LifxMetaData(lightJson["id"].ToString(), lightJson["uuid"].ToString());

                lights.Add(new DeviceImport(name, descriptor, meta.GetXmlString()));
            }

            return lights;
        }

        public async Task ExecuteFunction(UserContext context, DeviceImport deviceData, string function)
        {
            var token = context.GetLogin("appkey");
            var meta = LifxMetaData.FromString(deviceData.MetaData);
            await Post(new Uri($"https://api.lifx.com/v1/lights/{meta.Id}/toggle"), token, CancellationToken.None);
        }

        private static async Task<string> Get(Uri uri, string bearerToken, CancellationToken token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                var result = await client.GetAsync(uri, token);
                var str = await result.Content.ReadAsStringAsync();
                return await result.Content.ReadAsStringAsync();
            }
        }

        private static async Task<string> Post(Uri uri, string bearerToken, CancellationToken token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                var result = await client.PostAsync(uri, new StringContent(String.Empty), token);
                var str = await result.Content.ReadAsStringAsync();
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
