namespace HomeHub.Common.Exceptions
{
    using System.Net;

    /// <summary>
    /// When unauthorized access is attempted in the db
    /// </summary>
    public class ForbiddenDataAccessException : DataOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenDataAccessException"/> class.
        /// </summary>
        /// <param name="message">The message</param>
        public ForbiddenDataAccessException(string message) : base(message, "FORBIDDEN", HttpStatusCode.Forbidden)
        {
        }
    }
}
