using HomeHub.Service.Common.Data;
using HomeHub.Service.Common.Models;
using HomeHub.Service.Common.Security;
using HomeHub.Service.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HomeHub.Service.Web.Controllers
{
    /// <summary>
    /// The controller for devices
    /// </summary>
    [RequireCredentials]
    [RoutePrefix("device")]
    public class DeviceController : ApiController
    {
        /// <summary>
        /// Get all of your devices
        /// </summary>
        /// <returns>A list of all devices</returns>
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<DeviceModel>> GetDevices()
        {
            var guid = HttpContext.Current.User.Identity.UserId();
            return (await DataLayer.Instance.GetAllDevices(guid)).Select(d => new DeviceModel()
            {
                Name = d.Name,
                DeviceType = d.DeviceType,
                Actions = new []
                {
                    new DeviceActionModel()
                    {
                        Name = "Action 1",
                        Arguments = new []
                        {
                            new ActionArgumentModel()
                            {
                                Key = "Arg1",
                                Value = "Value1"
                            }
                        }
                    },
                    new DeviceActionModel()
                    {
                        Name = "Action 2",
                        Arguments = new []
                        {
                            new ActionArgumentModel()
                            {
                                Key = "Arg1",
                                Value = "Value1"
                            }
                        }
                    }
                }
            });
        }
    }
}
