using System.Net;
using HomeHub.Common.Exceptions;

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
        /// <typeparam name="T">
        /// The type to return
        /// </typeparam>
        /// <param name="stproc">
        /// The stored procedure.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="read">
        /// The read.
        /// </param>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<T> ExecuteSql<T>(string stproc, Action<SqlParameterCollection> parameters, Func<SqlDataReader, T> read, CancellationToken token)
        {
            HomeHubEventSource.Log.MethodEnter();

            HomeHubEventSource.Log.FetchingData(stproc);

            var result = default(T);

            using (var connection = new SqlConnection(this.connectionString))
            {
                await connection.OpenAsync(token);
                var command = connection.CreateCommand();
                command.CommandText = stproc;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                parameters(command.Parameters);

                await SqlConnectionManager.ExecSafe(async () =>
                {
                    using (var reader = await command.ExecuteReaderAsync(token))
                    {
                        result = read(reader);
                    }
                });
            }

            HomeHubEventSource.Log.MethodLeave();

            return result;
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

                await SqlConnectionManager.ExecSafe(async () =>
                {
                    using (var reader = await command.ExecuteReaderAsync(token))
                    {
                        read(reader);
                    }
                });
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

                await SqlConnectionManager.ExecSafe(() => command.ExecuteNonQueryAsync(token));
            }

            HomeHubEventSource.Log.MethodLeave();
        }

        private static async Task ExecSafe(Func<Task> action)
        {
            var logMessage = string.Empty;
            var exRethrow = default(Exception);

            try
            {
                await action();
            }
            catch (SqlException ex)
            {
                // handle sql exceptions
                switch (ex.Number)
                {
                    case 50001:
                        // NOT FOUND
                        exRethrow = new DuplicateItemException(ex.Message);
                        break;
                    case 50002:
                        // ACCESS DENIED
                        exRethrow = new UnauthorizedDataAccessException(ex.Message);
                        break;
                    case 50003:
                        // DUPLICATE
                        exRethrow = new DuplicateItemException(ex.Message);
                        break;
                    default:
                        // Well something horrible has gone wrong
                        exRethrow = new FailureException(ex.Message, "INTERNAL ERROR",
                            HttpStatusCode.InternalServerError);
                        break;
                }

                ExceptionUtility.TraceException(exRethrow);
                HomeHubEventSource.Log.Error($"Sql server error. Transaction rollback. {ex.Message}");
                throw exRethrow;
            }
        }
    }
}
