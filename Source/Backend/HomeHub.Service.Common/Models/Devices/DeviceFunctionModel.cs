﻿using HomeHub.Common.Devices;

namespace HomeHub.Service.Common.Models.Devices
{
    /// <summary>
    /// The device function model.
    /// </summary>
    public class DeviceFunctionModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceFunctionModel"/> class.
        /// </summary>
        /// <param name="function">The function.</param>
        public DeviceFunctionModel(DeviceFunction function)
        {
            this.Name = function.Name;
        }

        /// <summary>
        /// The name of this function
        /// </summary>
        public string Name { get; set; }
    }
}