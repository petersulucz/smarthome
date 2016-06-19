namespace HomeHub.Data.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HomeHub.Adapters.Common;

    /// <summary>
    /// The AccountLayer interface.
    /// </summary>
    public interface IAccountLayer
    {
        /// <summary>
        /// The add account.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="loginMeta">The login meta.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<UserContext> AddAccount(Guid user, string manufacturer, Dictionary<string, string> loginMeta);

        /// <summary>
        /// The get account.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<UserContext> GetAccount(Guid user, string manufacturer);
    }
}
