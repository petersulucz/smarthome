﻿namespace HomeHub.Common.Devices
{
    using System;

    /// <summary>
    /// The device.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="home">The home.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="definition">The definition.</param>
        /// <param name="meta">The device metadata.</param>
        public Device(Guid id, Guid home, string name, string description, DeviceDefinition definition, string meta)
        {
            this.Id = id;
            this.Home = home;
            this.Name = name;
            this.Description = description;
            this.Definition = definition;
            this.Meta = meta;
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        public DeviceState State { get; private set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the home.
        /// </summary>
        public Guid Home { get; private set; }

        /// <summary>
        /// Gets the meta.
        /// </summary>
        public string Meta { get; private set; }

        /// <summary>
        /// Gets the definition.
        /// </summary>
        public DeviceDefinition Definition { get; private set; }
    }
}
