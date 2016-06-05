namespace HomeHub.Common.Exceptions
{
    using System.Net;

    /// <summary>
    /// The data operation exception class
    /// </summary>
    public abstract class DataOperationException : FailureException
    {
        /// <summary>
        /// Represents a data operation failure
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="slug">The message slug</param>
        /// <param name="correspondingHttpStatusCode">The status code to use</param>
        protected DataOperationException(string message, string slug, HttpStatusCode correspondingHttpStatusCode) : base(message, slug, correspondingHttpStatusCode)
        {
        }
    }
}
