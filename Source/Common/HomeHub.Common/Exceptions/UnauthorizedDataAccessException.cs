namespace HomeHub.Common.Exceptions
{
    using System.Net;

    /// <summary>
    /// When unauthorized access is attempted in the db
    /// </summary>
    public class UnauthorizedDataAccessException : DataOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedDataAccessException"/> class.
        /// </summary>
        /// <param name="message">The message</param>
        public UnauthorizedDataAccessException(string message) : base(message, "UNAUTHORIZED", HttpStatusCode.Unauthorized)
        {
        }
    }
}
