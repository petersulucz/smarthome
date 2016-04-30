using HomeHub.Common.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public static class ExceptionUtility
    {
        private static void TraceException(Exception e)
        {
            HomeHubEventSource.Log.Error(e.Message);
        }

        /// <summary>
        /// Calls Failfast. Dont fuck with this. justin.
        /// </summary>
        /// <param name="message">Last words?</param>
        public static void ShootSelfInFace(string message)
        {
            HomeHubEventSource.Log.Critical(message);
            Environment.FailFast(message);
        }

        public static void EnsureArugmentNotNull(string name, object argument, string message = null)
        {
            if(null == argument)
            {
                var e = new ArgumentNullException(name, message);
                ExceptionUtility.TraceException(e);
                throw e;
            }
        }
    }
}
