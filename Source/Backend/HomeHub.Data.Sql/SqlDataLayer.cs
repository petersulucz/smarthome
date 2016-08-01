namespace HomeHub.Data.Sql
{
    using System;
    using System.Data;
    using System.Threading;

    using HomeHub.Data.Common;
    using HomeHub.Data.Common.Security;

    public partial class SqlDataLayer : IDataLayer, IDisposable
    {
        private readonly SqlConnectionManager connectionManager;

        private readonly CancellationTokenSource tokenSource;

        public SqlDataLayer(string connectionString)
        {
            this.connectionManager = new SqlConnectionManager(connectionString);
            this.tokenSource = new CancellationTokenSource();
        }

        /// <summary>
        ///     Dispose everything
        /// </summary>
        public void Dispose()
        {
            this.tokenSource.Dispose();
        }


        /// <summary>
        ///     Get a roles list datatable
        /// </summary>
        /// <param name="roles">The list of roles</param>
        /// <returns>The dt containing the roles</returns>
        private static DataTable GenerateRolesList(UserRoles roles)
        {
            var datatable = new DataTable();
            datatable.Columns.Add("claim");

            foreach (var role in Enum.GetValues(typeof(UserRoles)))
            {
                if (roles.HasFlag((UserRoles)role))
                {
                    datatable.Rows.Add(((UserRoles)role).ToString());
                }
            }

            return datatable;
        }
    }
}