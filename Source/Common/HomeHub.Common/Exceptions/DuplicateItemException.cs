namespace HomeHub.Common.Exceptions
{
    using System.Net;

    /// <summary>
    /// When a duplicate issue is detected in the db
    /// </summary>
    public class DuplicateItemException : DataOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateItemException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public DuplicateItemException(string message) : base(message, "DUPLICATE ITEM", HttpStatusCode.Conflict)
        {
        }
    }
}
