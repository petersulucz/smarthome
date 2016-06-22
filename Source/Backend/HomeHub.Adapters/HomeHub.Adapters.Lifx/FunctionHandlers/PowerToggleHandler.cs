namespace HomeHub.Adapters.Lifx.FunctionHandlers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using HomeHub.Adapters.Common;
    using HomeHub.Common.Devices;

    /// <summary>
    /// The power toggle handler.
    /// </summary>
    internal class PowerToggleHandler : FunctionHandler
    {
        public PowerToggleHandler()
            : base(new DeviceFunction("power", FunctionArgumentType.None))
        {
        }

        protected override Task execute(UserContext context, LifxMetaData meta, object argument, HttpClient client)
        {
            return client.PostAsync($"https://api.lifx.com/v1/lights/{meta.Id}/toggle", new StringContent(String.Empty));
        }
    }
}
