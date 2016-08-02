using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Common.Exceptions
{
    using System.Net;

    public class UnauthorizedDataAccessException : DataOperationException
    {
        public UnauthorizedDataAccessException(string message)
            : base(message, "UNAUTHORIZED", HttpStatusCode.Unauthorized)
        {
        }
    }
}
