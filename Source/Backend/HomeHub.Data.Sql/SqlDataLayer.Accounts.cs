namespace HomeHub.Data.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using HomeHub.Adapters.Common;
    using HomeHub.Data.Common;

    public partial class SqlDataLayer : IAccountLayer
    {
        async Task<UserContext> IAccountLayer.AddAccount(Guid user, Guid home, string manufacturer, Dictionary<string, string> loginMeta)
        {
            var usercontext = new UserContext(user, manufacturer, null);
            foreach (var keyval in loginMeta)
            {
                usercontext.AddLoginContext(keyval.Key, keyval.Value);
            }

            var doc = usercontext.SerializeLogin().ToString();

            await this.connectionManager.ExecuteSql(
                "hub.addaccountlogin",
                collection =>
                {
                    collection.AddWithValue("user", user);
                    collection.AddWithValue("home", home);
                    collection.AddWithValue("manufacturer", manufacturer);
                    collection.AddWithValue("meta", doc);
                }, this.tokenSource.Token);

            return usercontext;
        }

        async Task<IEnumerable<UserContext>> IAccountLayer.GetAccount(Guid user, Guid home)
        {
            return await this.connectionManager.ExecuteSql(
                "hub.getaccountlogins",
                collection =>
                {
                    collection.AddWithValue("user", user);
                    collection.AddWithValue("home", home);
                },
                reader =>
                {
                    var contexts = new List<UserContext>();
                    while (reader.Read())
                    {
                        var meta = (string)reader["meta"];
                        var manufacturer = (string)reader["manufacturer"];
                        var accountUser = (Guid)reader["user"];

                        var doc = XDocument.Parse(meta);
                        var usercontext = new UserContext(accountUser, manufacturer, null);
                        usercontext.Load(doc);
                        contexts.Add(usercontext);
                    }
                    return contexts;
                },
                this.tokenSource.Token);
        }

    }
}
