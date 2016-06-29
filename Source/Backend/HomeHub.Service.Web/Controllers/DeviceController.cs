using HomeHub.Service.Common.Models.Devices;

namespace HomeHub.Service.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using HomeHub.Service.Common.Data;
    using HomeHub.Service.Common.Models;
    using HomeHub.Service.Common.Security;
    using HomeHub.Service.Web.Models;
    using HomeHub.Service.Web.Pipeline.Filters;

    /// <summary>
    /// This is the controller for all devices. It is used to access and perform operations on each device.
    /// This is probably one of the most important controllers to use. 
    /// </summary>
    [RequireCredentials]
    [RoutePrefix("device")]
    public class DeviceController : ApiController
    {
        /// <summary>
        /// Get all of your devices attached to a home.
        /// To come, add some sort of search functionality here.
        /// Maybe then finally justin will be happy. But for now, gets every device attached to a home.
        /// </summary>
        /// <param name="home">The guid for the home to list all of the devices for.</param>
        /// <returns>A list of all devices. Includes their definitions and functions and stuff.</returns>
        [HttpGet]
        [Route("{home}")]
        public async Task<IEnumerable<DeviceModel>> GetDevices([FromUri] Guid home)
        {
            // who knows wtf they are trying to do. throw exception
            if (default(Guid) == home)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var user = HttpContext.Current.User.Identity.UserId();
            var devs = await DataLayer.Instance.GetAllDevices(user, home);

            return devs.Select(d => new DeviceModel(d));
        }

        /// <summary>
        /// Create a new device
        /// </summary>
        /// <param name="model">The model</param>
        /// <returns>The created device</returns>
        //[HttpPost]
        //[Route("")]
        //public async Task<DeviceModel> CreateDevice(NewDeviceModel model)
        //{
        //    var device = await DataLayer.Instance.CreateDevice(model.Name, model.Home, model.Description, model.DeviceDefinition);

        //    return new DeviceModel(device);
        //}

        /// <summary>
        /// Get all of the device definitions available
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("definitions")]
        public async Task<Dictionary<string, IEnumerable<DeviceDefinitionModel>>> GetDefinitions()
        {
            var definitions = await DataLayer.Instance.GetDefinitions();

            return definitions.Keys.ToDictionary(key => key, key => definitions[key].Select(def => new DeviceDefinitionModel(def)));
        }

        /// <summary>
        /// Execute an action on a device
        /// </summary>
        /// <param name="device">The device to execute the action on.</param>
        /// <param name="func">The function to execute.</param>
        /// <param name="argument">The argument. If no argument. Can pass either empty, or no body</param>
        /// <returns>No content. Or an error.</returns>
        [HttpPut]
        [Route("exec/{device}/{func}")]
        public async Task Execute([FromUri] Guid device, [FromUri] string func, [FromBody] FunctionArgumentModel argument)
        {
            var user = HttpContext.Current.User.Identity.UserId();
            await DeviceManager.ExecuteAction(user, device, func, argument?.Argument);
        }
    }
}
