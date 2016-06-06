
namespace HomeHub.Service.Web.Filters
{
    using System.Net.Http;
    using System.Web.Http.Filters;

    using HomeHub.Common.Exceptions;

    /// <summary>
    /// An exception filter for failure exceptions
    /// </summary>
    public class FailureExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        /// <summary>Raises the exception event.</summary>
        /// <param name="actionExecutedContext">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is FailureException)
            {
                var failure = (FailureException)actionExecutedContext.Exception;
                actionExecutedContext.Response = new HttpResponseMessage(failure.HttpCode)
                                                     {
                                                         ReasonPhrase = failure.Slug,
                                                         Content =
                                                             new StringContent(
                                                             failure.Message)
                                                     };
            }

            base.OnException(actionExecutedContext);
        }
    }
}