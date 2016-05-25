using HomeHub.Common.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Common.Exceptions
{
    using System.IO;

    [ExcludeFromCodeCoverage]
    public static class ExceptionUtility
    {
        private static void TraceException(Exception e)
        {
            HomeHubEventSource.Log.Error(e.Message);
        }

        /// <summary>
        /// Calls Failfast.
        /// </summary>
        /// <param name="message">Last words?</param>
        public static void ShootSelfInFace(string message)
        {
            HomeHubEventSource.Log.Critical(message);
            Environment.FailFast(message);
        }

        /// <summary>
        /// Throws an argument exception if false
        /// </summary>
        /// <param name="value">The value which could be false</param>
        /// <param name="message">The message</param>
        public static void ThrowArgumentExceptionIfFalse(bool value, string message)
        {
            if (false == value)
            {
                var e = new ArgumentException(message);
                ExceptionUtility.TraceException(e);
                throw e;
            }
        }

        /// <summary>
        /// Throws an invalid data exception
        /// </summary>
        /// <param name="reason">The reason for the exception</param>
        public static void ThrowInvalidDataException(string reason)
        {
            var e = new InvalidDataException(reason);
            ExceptionUtility.TraceException(e);
            throw e;
        }

        /// <summary>
        /// Ensure an argument is not null
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argument"></param>
        /// <param name="message"></param>
        public static void EnsureArugmentNotNull(string name, object argument, string message = null)
        {
            if(null == argument)
            {
                var e = new ArgumentNullException(name, message);
                ExceptionUtility.TraceException(e);
                throw e;
            }
        }

        public static void ThrowFailureException(string reason)
        {
            var e = new FailureException(reason);
            ExceptionUtility.TraceException(e);
            throw e;
        }

    }
}
