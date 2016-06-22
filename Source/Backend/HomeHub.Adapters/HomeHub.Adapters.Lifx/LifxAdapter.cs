namespace HomeHub.Adapters.Lifx
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using HomeHub.Adapters.Common;
    using HomeHub.Adapters.Lifx.FunctionHandlers;
    using HomeHub.Common.Devices;
    using HomeHub.Common.Exceptions;

    using Newtonsoft.Json.Linq;

    using static HomeHub.Common.Trace.HomeHubEventSource;

    [Export(typeof(IHomeHubAdapter))]
    public class LifxAdapter : IHomeHubAdapter
    {
        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        string IHomeHubAdapter.Manufacturer => "lifx";

        /// <summary>
        /// Check if two devices are the same devices within griddle
        /// </summary>
        /// <param name="a">The a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// True if the devices are the same physical device. False otherwise.
        /// </returns>
        public bool Compare(DeviceImport a, DeviceImport b)
        {
            var aData = LifxMetaData.FromString(a.MetaData);
            var bData = LifxMetaData.FromString(b.MetaData);

            return string.Equals(aData.Id, bData.Id, StringComparison.Ordinal)
                   && string.Equals(aData.UUID, bData.UUID, StringComparison.Ordinal);
        }

        /// <summary>
        /// The get devices.
        /// </summary>
        /// <param name="context">The user context.</param>
        /// <returns>
        /// A task.
        /// </returns>
        public async Task<IEnumerable<DeviceImport>> GetDevices(UserContext context)
        {
            var token = context.GetLogin("appkey");
            var lights = new List<DeviceImport>();

            // Ask lifx for all data
            using (var client = Helpers.GetClient(token))
            {
                var response = await client.GetAsync(new Uri("https://api.lifx.com/v1/lights/all"), CancellationToken.None);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Log.Error($"Failed to retreive devices");
                    ExceptionUtility.ThrowFailureException($"Could not load data for user.");
                }

                var str = await response.Content.ReadAsStringAsync();

                // We are only getting lights
                var lightJsonArray = JArray.Parse(str);

                lights.AddRange(lightJsonArray.Select(Helpers.LoadFromJson));
            }

            return lights;
        }

        /// <summary>
        /// Execute a function on a device
        /// </summary>
        /// <param name="context">The user context.</param>
        /// <param name="deviceData">The device data.</param>
        /// <param name="function">The function to execute.</param>
        /// <param name="argument">The argument to the function</param>
        /// <returns>
        /// A task.
        /// </returns>
        public Task ExecuteFunction(UserContext context, DeviceImport deviceData, DeviceFunction function, object argument)
        {
            // Get the device meta for execute
            var meta = LifxMetaData.FromString(deviceData.MetaData);

            return Handlers.ExecuteFunction(context, meta, function, argument);
        }


    }
}
