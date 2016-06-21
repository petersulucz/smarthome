namespace HomeHub.Common.Devices
{
    /// <summary>
    /// The device function.
    /// </summary>
    public class DeviceFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceFunction"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="argumentType">The type of argument the function takes</param>
        public DeviceFunction(string name, FunctionArgumentType argumentType)
        {
            this.Name = name;
            this.ArgumentType = argumentType;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the argument type.
        /// </summary>
        public FunctionArgumentType ArgumentType { get; private set; }
    }
}
