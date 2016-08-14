namespace HomeHub.Administration.Common.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The home hub test connection model.
    /// </summary>
    [DataContract]
    public class HomeHubTestConnectionCom
    {
        public HomeHubTestConnectionCom(string hello)
        {
            this.TestString = hello;
        }

        /// <summary>
        /// Gets or sets the test string.
        /// </summary>
        [DataMember(Order = 1)]
        public string TestString { get; set; }
    }
}
