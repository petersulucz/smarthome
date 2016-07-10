namespace HomeHub.Adapters.Lifx
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;

    using HomeHub.Common.Devices;
    using HomeHub.Common.Devices.Light;
    using HomeHub.Common.Exceptions;

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

            var state = Helpers.LoadDeviceState(json);

            return new DeviceImport(name, descriptor, meta.GetXmlString(), state);
        }

        /// <summary>
        /// The load device state.
        /// </summary>
        /// <param name="json">
        /// The json object to pull all of the device state from
        /// </param>
        /// <returns>
        /// The <see cref="DeviceState"/>.
        /// </returns>
        private static DeviceState LoadDeviceState(JToken json)
        {
            var isConnected = json["connected"].ToObject<bool>();
            var isOn = json["power"].ToString().Equals("on", StringComparison.OrdinalIgnoreCase);
            var hugh = json["color"]["hue"].ToObject<double>();
            var saturation = json["color"]["saturation"].ToObject<double>();
            var brightNess = json["brightness"].ToObject<double>();
            var color = Helpers.GetRgb(hugh, saturation, brightNess);
            return new LightState(isConnected, isOn, color);
        }

        /// <summary>
        /// Literally dont tough this.
        /// </summary>
        /// <param name="hugh">The hue</param>
        /// <param name="saturation">the Saturation</param>
        /// <param name="brightness">The brightness</param>
        /// <returns>The color as an int</returns>
        private static int GetRgb(double hugh, double saturation, double brightness)
        {
            if (hugh < 0 || hugh > 360)
            {
                ExceptionUtility.ThrowInvalidDataException("Hugh is not in rage");
            }

            if (saturation < 0 || saturation > 1.0)
            {
                ExceptionUtility.ThrowInvalidDataException("Saturation is not in range");
            }

            if (brightness < 0 || brightness > 1.0)
            {
                ExceptionUtility.ThrowInvalidDataException("Brightness is not in range");
            }

            var c = (1 - Math.Abs(2.0 * brightness - 1)) * saturation;
            hugh /= 60.0;
            var x = c * (1 - Math.Abs(hugh % 2 - 1));
            var m = brightness - 0.5 * c;

            var rgb1 = new[] { m, m, m };
            rgb1[(7 - (int)hugh) % 3] += x;
            rgb1[(int)Math.Ceiling(((int)hugh % 5) / 2.0)] += c;

            return (int)(rgb1[0] * 256) << 16 | (int)(rgb1[1] * 256) << 8 | (int)(rgb1[2] * 256);
        }
    }
}
