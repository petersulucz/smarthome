namespace HomeHub.Data.Common.Models.Security
{
    using HomeHub.Data.Common.Security;

    public class AuthenticationToken
    {
        /// <summary>
        /// The token
        /// </summary>
        public virtual string Token { get; set; }

        /// <summary>
        /// The list of claims
        /// </summary>
        public virtual UserRoles Claims { get; set; }

    }
}
