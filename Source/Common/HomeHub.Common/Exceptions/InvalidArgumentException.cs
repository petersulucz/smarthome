namespace HomeHub.Common.Exceptions
{
    using System.Net;

    /// <summary>
    /// The invalid argument exception.
    /// </summary>
    public class InvalidArgumentException : FailureException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class. 
        /// </summary>
        /// <param name="message">
        /// The message
        /// </param>
        public InvalidArgumentException(string message)
            : base(message, "ARGUMENT INVALID", HttpStatusCode.BadRequest)
        {
        }
    }
}
