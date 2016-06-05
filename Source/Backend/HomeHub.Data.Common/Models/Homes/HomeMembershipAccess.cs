using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Data.Common.Models.Homes
{
    [Flags()]
    public enum HomeMembershipAccess : byte
    {
        /// <summary>
        /// Look at the home but do nothing else
        /// </summary>
        Read = 0,

        /// <summary>
        /// Basic on off controls
        /// </summary>
        Basic = 0x1,

        /// <summary>
        /// Not sure what this flag means for now, but thinking its for cool features
        /// </summary>
        Advanced = 0x2,

        /// <summary>
        /// Can create devices on a home
        /// </summary>
        Create = 0x4,

        /// <summary>
        /// Can delete devices on a home
        /// </summary>
        Delete = 0x8,

        /// <summary>
        /// Special admin access
        /// </summary>
        Admin = 0x80
    }
}
