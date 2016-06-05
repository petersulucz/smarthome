namespace HomeHub.Common.Exceptions
{
    using System;
    using System.Net;

    /// <summary>
    /// A general failure
    /// </summary>
    public class FailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataOperationException"/> class.
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="slug">The message slug</param>
        /// <param name="correspondingHttpStatusCode">The http response code to use if this makes it all the way up</param>
        public FailureException(string message, string slug, HttpStatusCode correspondingHttpStatusCode)
            : base(message)
        {
            this.Slug = slug;
            this.HttpCode = correspondingHttpStatusCode;
        }

        /// <summary>
        /// Gets the http status code to throw for this exception
        /// </summary>
        public HttpStatusCode HttpCode { get; private set; }

        /// <summary>
        /// Gets the short message slug. To usually use as an http reason phrase
        /// </summary>
        public string Slug { get; private set; }
    }
}
