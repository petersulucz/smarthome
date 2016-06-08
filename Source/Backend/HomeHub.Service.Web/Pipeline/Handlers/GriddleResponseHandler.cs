namespace HomeHub.Service.Web.Pipeline.Handlers
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using HomeHub.Service.Web.Models;

    /// <summary>
    /// Adds the griddle response format to every response
    /// </summary>
    public class GriddleResponseHandler : DelegatingHandler
    {
        /// <summary>Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.</summary>
        /// <returns>Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.</returns>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> was null.</exception>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(
                task =>
                    {
                        // We have the response message
                        var response = task.Result;

                        return this.HandleResponseMessage(response);
                    }, cancellationToken);
        }

        private HttpResponseMessage HandleResponseMessage(HttpResponseMessage response)
        {
            var contentType = response.Content.Headers.ContentType?.MediaType;

            if (null == contentType)
            {
                // Dump the response back out
                // literally no idea what to do here
                return response;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                // Non success, just spit it out
                return response;
            }

            // Only wraps json for now. Assuming people using xml are too badass for fancy wrapper objects
            return true == contentType.Contains("json") ? GetWrappedJsonResponse(response) : response;
        }

        /// <summary>
        /// Gets wrapped json content
        /// </summary>
        /// <param name="message">The http response message to pull stuff out of</param>
        /// <returns>The same http response message, but with new content</returns>
        private static HttpResponseMessage GetWrappedJsonResponse(HttpResponseMessage message)
        {
            var content = (ObjectContent)message.Content;
            message.Content = GetGriddleResponse(content);
            return message;
        }

        /// <summary>
        /// Get a griddle response with extra params
        /// </summary>
        /// <param name="payload">The payload to wrap in a griddle</param>
        /// <returns>A new object content</returns>
        private static ObjectContent GetGriddleResponse(ObjectContent payload)
        {
            // Lets pretend this is not a huge performance penalty JUSTIN.
            // HERE IS YOUR STUPID WRAPPED JSON RESPONSE
            var payloadType = payload.ObjectType;
            var generic = typeof(GriddleResponse<>).MakeGenericType(payloadType);
            var constructor = generic.GetConstructor(new[] { payloadType });
            var griddleResponse = constructor.Invoke(new [] { payload.Value});

            // Passing the original formatter in here. For json it seems fine, but the second I choose xml.... nooooooooo
            // TODO: will look into this later
            return new ObjectContent(generic, griddleResponse, payload.Formatter);
        }
    }
}