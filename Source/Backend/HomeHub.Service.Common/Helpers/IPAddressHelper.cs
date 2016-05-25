using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HomeHub.Service.Common.Helpers
{
    public static class IPAddressHelper
    {
        public static IPAddress GetIPAddress(HttpRequestMessage request)
        {
            var context = request.Properties["MS_HttpContext"] as HttpContextWrapper;

            if (null == context)
            {
                return IPAddress.IPv6None;
            }

            return IPAddress.Parse(context.Request.UserHostAddress);
        }
    }
}
