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
    using HomeHub.Service.Web.Security;

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
        /// <param name="home">Id of the home to get devices for</param>
        /// <returns>A list of all devices</returns>
        [HttpGet]
        [Route("{home}")]
        public async Task<IEnumerable<DeviceModel>> GetDevices(Guid home)
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
        [HttpPost]
        [Route("")]
        public async Task<DeviceModel> CreateDevice(NewDeviceModel model)
        {
            var device = await DataLayer.Instance.CreateDevice(model.Name, model.Home, model.Description, model.DeviceDefinition);

            return new DeviceModel(device);
        }

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
    }
}
