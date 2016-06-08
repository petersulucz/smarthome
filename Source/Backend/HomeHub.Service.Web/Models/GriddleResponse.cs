namespace HomeHub.Service.Web.Models
{
    /// <summary>
    /// A response from griddle
    /// </summary>
    public class GriddleResponse<T>
    {
        /// <summary>
        /// A standard griddle response
        /// </summary>
        /// <param name="content">The content of the griddle response</param>
        public GriddleResponse(T content)
        {
            this.Data = content;
            this.Version = "0.0.0.1";
        }

        /// <summary>
        /// The data payload
        /// </summary>
        public T Data { get; private set; }

        /// <summary>
        /// The api version
        /// </summary>
        public string Version { get; private set; }
    }
}