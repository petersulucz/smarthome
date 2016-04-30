using HomeHub.Common.Trace;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHub.Data.Sql
{
    internal static class SqlConnectionManager
    {
        private static string ConnectionString
        {
            get
            {
                return "";
            }
        }

        public static async Task ExecuteSql(string stproc, Action<SqlParameterCollection> parameters, Action<SqlDataReader> read, CancellationToken token)
        {
            HomeHubEventSource.Log.MethodEnter();

            HomeHubEventSource.Log.FetchingData(stproc);

            using (var connection = new SqlConnection(SqlConnectionManager.ConnectionString))
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

        public static async Task ExecuteSql(string stproc, Action<SqlParameterCollection> parameters, CancellationToken token)
        {
            HomeHubEventSource.Log.MethodEnter();

            HomeHubEventSource.Log.FetchingData(stproc);

            using (var connection = new SqlConnection(SqlConnectionManager.ConnectionString))
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
