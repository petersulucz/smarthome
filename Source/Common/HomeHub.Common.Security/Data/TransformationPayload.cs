namespace HomeHub.Common.Security.Data
{
    using System;

    /// <summary>
    /// The transformation payload.
    /// </summary>
    [Serializable]
    internal class TransformationPayload
    {
        /// <summary>
        /// Gets or sets the finger print.
        /// </summary>
        public string FingerPrint { get; set; }

        /// <summary>
        /// Gets or sets the xml content.
        /// </summary>
        public string XmlContent { get; set; }
    }
}
