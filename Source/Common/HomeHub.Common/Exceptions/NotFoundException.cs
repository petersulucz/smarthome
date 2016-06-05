namespace HomeHub.Common.Exceptions
{
    using System.Net;

    /// <summary>
    /// When something is not found in the db
    /// </summary>
    public class NotFoundException : DataOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="message">Message to send</param>
        public NotFoundException(string message) : base(message, "NOT FOUND", HttpStatusCode.NotFound)
        {
        }
    }
}
