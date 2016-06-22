namespace HomeHub.Adapters.Lifx.FunctionHandlers
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using HomeHub.Adapters.Common;
    using HomeHub.Common.Devices;

    /// <summary>
    /// The function handler.
    /// </summary>
    internal abstract class FunctionHandler
    {
        /// <summary>
        /// Initializes a new instance of the function handler.
        /// </summary>
        /// <param name="function">The function</param>
        protected FunctionHandler(DeviceFunction function)
        {
            this.Key = $"{function.Name}";
        }

        /// <summary>
        /// Gets the key
        /// </summary>
        public string Key { get; private set; }

        public Task Execute(UserContext context, LifxMetaData meta, object argument)
        {
            var client = Helpers.GetClient(context.GetLogin("appkey"));

            return this.execute(context, meta, argument, client).ContinueWith(
                result =>
                    {
                        client.Dispose();
                        return result;
                    });
        }

        /// <summary>
        /// Execute the function on the device
        /// </summary>
        /// <param name="context">The user context info.</param>
        /// <param name="meta">The metadata</param>
        /// <param name="argument">The argument.</param>
        /// <param name="client">The http client.</param>
        /// <returns>A task.</returns>
        protected abstract Task execute(UserContext context, LifxMetaData meta, object argument, HttpClient client);
    }
}
