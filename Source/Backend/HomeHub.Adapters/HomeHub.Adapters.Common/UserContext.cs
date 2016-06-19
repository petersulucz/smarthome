namespace HomeHub.Adapters.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Linq;

    /// <summary>
    /// The user context.
    /// </summary>
    public class UserContext
    {
        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        public string Manufacturer { get; private set; }

        /// <summary>
        /// Gets the device name.
        /// </summary>
        public string DeviceName { get; private set; }

        /// <summary>
        /// Gets the user.
        /// </summary>
        public Guid User { get; private set; }

        /// <summary>
        /// The context.
        /// </summary>
        private readonly Dictionary<string, string> context = new Dictionary<string, string>();

        private readonly Dictionary<string, string> login = new Dictionary<string, string>();

        public UserContext(Guid user, string manufacturer, string deviceName)
        {
            this.Manufacturer = manufacturer;
            this.DeviceName = deviceName;
            this.User = user;
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string this[string key]
        {
            get
            {
                return this.context[key];
            }
            set
            {
                this.context[key] = value;
            }
        }

        /// <summary>
        /// The add login context.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void AddLoginContext(string key, string value)
        {
            this.login[key] = value;
        }

        /// <summary>
        /// The get login.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetLogin(string key)
        {
            return this.login[key];
        }

        /// <summary>
        /// The get login keys.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<string> GetLoginKeys()
        {
            return this.login.Keys;
        }

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <returns>
        /// The <see cref="XDocument"/>.
        /// </returns>
        internal XDocument SerializeLogin()
        {
            return new XDocument(new XElement("login", this.login.Select(kvp => new XElement(kvp.Key, kvp.Value))));
        }

        /// <summary>
        /// The serialize context.
        /// </summary>
        /// <returns>
        /// The <see cref="XDocument"/>.
        /// </returns>
        internal XDocument SerializeContext()
        {
            return new XDocument(new XElement("keyvalue", this.context.Select(kvp => new XElement(kvp.Key, kvp.Value))));
        }

        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="doc">
        /// The doc.
        /// </param>
        internal void Load(XDocument doc)
        {
            var loginElements = doc.Element("login");
            var keyvalueElements = doc.Element("keyvalue");

            if (null != loginElements)
            {
                foreach (var elem in loginElements.Elements())
                {
                    this.login[elem.Name.ToString()] = elem.Value;
                }
            }

            if (null != keyvalueElements)
            {
                foreach (var elem in keyvalueElements.Elements())
                {
                    this.context[elem.Name.ToString()] = elem.Value;
                }
            }
        }
    }
}
