namespace HomeHub.Adapters.Lifx.FunctionHandlers
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using HomeHub.Adapters.Common;
    using HomeHub.Common.Devices;

    /// <summary>
    /// The color handler.
    /// </summary>
    internal class ColorHandler : FunctionHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorHandler"/> class. 
        /// </summary>
        public ColorHandler()
            : base(new DeviceFunction("color", FunctionArgumentType.Int))
        {
        }

        /// <summary>
        /// Execute the function on the device
        /// </summary>
        /// <param name="context">The user context info.</param>
        /// <param name="meta">The metadata</param>
        /// <param name="argument">The argument.</param>
        /// <param name="client">The http client.</param>
        /// <returns>A task.</returns>
        protected override async Task execute(UserContext context, LifxMetaData meta, object argument, HttpClient client)
        {
            var uri = $"https://api.lifx.com/v1/lights/{meta.Id}/state";

            var colorInt = (int)argument;

            var colorStr = "#" + colorInt.ToString("X6");

            var content = new StringContent($"{{\"color\": \"{colorStr}\"}}");
            content.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            var result = await client.PutAsync(uri, content);

            result.EnsureSuccessStatusCode();

        }
    }
}
