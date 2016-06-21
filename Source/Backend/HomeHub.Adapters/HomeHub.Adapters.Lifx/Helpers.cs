namespace HomeHub.Adapters.Lifx
{
    using System.Net.Http;
    using System.Net.Http.Headers;

    using HomeHub.Common.Devices;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The helpers.
    /// </summary>
    internal static class Helpers
    {
        public static HttpClient GetClient(string bearer)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            return client;
        }

        /// <summary>
        /// Load a device from the json return
        /// </summary>
        /// <param name="json">The json string</param>
        /// <returns>A device import object</returns>
        public static DeviceImport LoadFromJson(JToken json)
        {
            var name = json["label"].ToString();

            var descriptor = json["product"]["identifier"].ToString();

            var meta = new LifxMetaData(json["id"].ToString(), json["uuid"].ToString());

            return new DeviceImport(name, descriptor, meta.GetXmlString());
        }
    }
}
