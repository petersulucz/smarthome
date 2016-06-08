using System;
using HomeHub.Service.Common.Models.Homes;

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
    using HomeHub.Service.Web.Models;
    using HomeHub.Service.Web.Pipeline.Filters;

    /// <summary>
    ///     Stuff with homes
    /// </summary>
    [RoutePrefix("homes")]
    [RequireCredentials]
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

        /// <summary>
        /// Get all of the users which have access to a home
        /// </summary>
        /// <param name="home">The home</param>
        /// <returns>The list of home memberships</returns>
        [HttpGet]
        [Route("{home}/access")]
        public async Task<IEnumerable<HomeMembershipModel>> GetUsers(Guid home)
        {
            var user = System.Web.HttpContext.Current.User.Identity.UserId();
            return (await DataLayer.Instance.GetHomeUsers(home, user)).Select(u => new HomeMembershipModel(u));
        }

        /// <summary>
        /// Add a user to a home
        /// </summary>
        /// <param name="home">The home id</param>
        /// <param name="model">The model of the home user</param>
        /// <returns>THe home membership model</returns>
        [HttpPost]
        [Route("{home}/access")]
        public async Task<HomeMembershipModel> AddUser([FromUri]Guid home, [FromBody]HomeMembershipModel model)
        {
            var user = System.Web.HttpContext.Current.User.Identity.UserId();
            var membership = await DataLayer.Instance.AddHomeUser(model.ToHomeMembership(), home, user);

            return new HomeMembershipModel(membership);
        }
    }
}