namespace HomeHub.Adapters.Lifx.FunctionHandlers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HomeHub.Adapters.Common;
    using HomeHub.Common.Devices;

    using static HomeHub.Common.Trace.HomeHubEventSource;

    /// <summary>
    /// The handlers.
    /// </summary>
    internal static class Handlers
    {
        /// <summary>
        /// The handlers dictionary.
        /// </summary>
        private static readonly Dictionary<string, FunctionHandler> FunctionHandlers = new Dictionary<string, FunctionHandler>();

        static Handlers()
        {
            Log.MethodEnter();
            Handlers.AddFunction(new PowerToggleHandler());

            Log.MethodLeave();
        }

        /// <summary>
        /// Run the function
        /// </summary>
        /// <param name="context">The user context.</param>
        /// <param name="meta">The light meta data.</param>
        /// <param name="function">The function to execute.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static Task ExecuteFunction(UserContext context, LifxMetaData meta, DeviceFunction function)
        {
            Log.MethodEnter();
            Log.Info($"Executing function {function.Name} on device {meta.Id}");
            var handler = Handlers.FunctionHandlers[function.Name];
            var task = handler.Execute(context, meta);
            Log.MethodLeave();
            return task;
        }

        /// <summary>
        /// A a function to the dictionary.
        /// </summary>
        /// <param name="handler">The handler.</param>
        private static void AddFunction(FunctionHandler handler)
        {
            Log.Info($"Adding handler for {handler.Key}");
            Handlers.FunctionHandlers[handler.Key] = handler;
        }
    }
}
