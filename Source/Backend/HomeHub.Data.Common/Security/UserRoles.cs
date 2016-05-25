using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Data.Common.Security
{
    [Flags]
    public enum UserRoles
    {
        BASIC = 1,
        ADMIN = 2
    }
}
