namespace HomeHub.Data.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HomeHub.Common.Devices;
    using HomeHub.Data.Common.Models;

    /// <summary>
    /// The DataLayer interface.
    /// </summary>
    public interface IDataLayer : IHomeLayer, IDeviceLayer, IDeviceDefinitions
    {

    }
}
