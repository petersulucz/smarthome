using System;
using System.Linq;

namespace HomeHub.Service.Web.Exceptions
{
    using System.Net;
    using System.Web.Http.ModelBinding;

    using HomeHub.Common.Exceptions;
    /// <summary>
    /// A model validation exception
    /// </summary>
    public class ModelValidationException : FailureException
    {
        /// <summary>
        /// A model validation exception
        /// </summary>
        /// <param name="dictionary">The model state</param>
        public ModelValidationException(ModelStateDictionary dictionary)
            : base(ModelValidationException.GetMessage(dictionary), "INVALID MODEL", HttpStatusCode.BadRequest)
        {
        }

        private static string GetMessage(ModelStateDictionary dictionary)
        {
            return dictionary.Aggregate(
                "The following stuff is wrong:",
                (current, kvp) =>
                current
                + (Environment.NewLine
                   + $"{kvp.Key}: {string.Join(", ", kvp.Value.Errors.Select(error => error.ErrorMessage))}"));
        }
    }
}