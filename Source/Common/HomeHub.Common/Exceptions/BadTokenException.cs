namespace HomeHub.Common.Exceptions
{
    using System.Net;

    /// <summary>
    /// The bad token exception.
    /// </summary>
    public class BadTokenException : FailureException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadTokenException"/> class. 
        /// </summary>
        /// <param name="message">The message to send to the user</param>
        public BadTokenException(string message)
            : base(message, "BAD TOKEN", HttpStatusCode.Unauthorized)
        {
        }
    }
}
