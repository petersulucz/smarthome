namespace HomeHub.Data.Sql
{
    using System;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    using HomeHub.Common.Trace;

    /// <summary>
    /// The SQL connection manager.
    /// </summary>
    internal class SqlConnectionManager
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConnectionManager"/> class. 
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        public SqlConnectionManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// The execute SQL.
        /// </summary>
        /// <param name="stproc">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="read">The read.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task ExecuteSql(string stproc, Action<SqlParameterCollection> parameters, Action<SqlDataReader> read, CancellationToken token)
        {
            HomeHubEventSource.Log.MethodEnter();

            HomeHubEventSource.Log.FetchingData(stproc);

            using (var connection = new SqlConnection(this.connectionString))
            {
                await connection.OpenAsync(token);
                var command = connection.CreateCommand();
                command.CommandText = stproc;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                parameters(command.Parameters);

                using (var reader = await command.ExecuteReaderAsync(token))
                {
                    read(reader);
                }
            }

            HomeHubEventSource.Log.MethodLeave();
        }

        /// <summary>
        /// Execute the SQL.
        /// </summary>
        /// <param name="stproc">The stored procedure name.</param> 
        /// <param name="parameters">The parameters.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task ExecuteSql(string stproc, Action<SqlParameterCollection> parameters, CancellationToken token)
        {
            HomeHubEventSource.Log.MethodEnter();

            HomeHubEventSource.Log.FetchingData(stproc);

            using (var connection = new SqlConnection(this.connectionString))
            {
                await connection.OpenAsync(token);
                var command = connection.CreateCommand();
                command.CommandText = stproc;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                parameters(command.Parameters);

                await command.ExecuteNonQueryAsync(token);
            }

            HomeHubEventSource.Log.MethodLeave();
        }
    }
}
