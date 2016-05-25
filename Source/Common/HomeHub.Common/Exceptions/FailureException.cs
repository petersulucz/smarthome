using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Common.Exceptions
{
    public class FailureException : Exception
    {
        /// <summary>
        /// A failure exception
        /// </summary>
        /// <param name="reason"></param>
        public FailureException(string reason)
            : base(reason)
        {
            
        }
    }
}
