namespace HomeHub.Service.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using HomeHub.Service.Common.Data;
    using HomeHub.Service.Common.Models;
    using HomeHub.Service.Common.Security;

    /// <summary>
    ///     Stuff with homes
    /// </summary>
    [RoutePrefix("homes")]
    public class HomesController : ApiController
    {
        /// <summary>
        ///     Create a new home
        /// </summary>
        /// <param name="newHome">The new home object</param>
        /// <returns>The new home result</returns>
        [HttpPost]
        [Route("")]
        public async Task<HomeModel> CreateHome(NewHomeModel newHome)
        {
            if (null == newHome)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var guid = System.Web.HttpContext.Current.User.Identity.UserId();
            var res = await DataLayer.Instance.CreateHome(newHome.ToHome(), guid);

            return new HomeModel(res);
        }

        /// <summary>
        ///     Get all of the homes you have access to
        /// </summary>
        /// <returns>All the homes you have access to</returns>
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<HomeModel>> GetHomes()
        {
            var guid = System.Web.HttpContext.Current.User.Identity.UserId();
            return (await DataLayer.Instance.GetHomes(guid)).Select(h => new HomeModel(h));
        }
    }
}