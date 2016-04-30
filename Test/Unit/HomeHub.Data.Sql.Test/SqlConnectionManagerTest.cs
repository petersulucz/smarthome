using System;
using System.Data.Fakes;
using System.Data.SqlClient.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using Microsoft.QualityTools.Testing.Fakes;
using System.Threading.Tasks;
using HomeHub.Data.Sql;
using System.Threading;
using System.Data;

namespace HomeHub.Data.Sql.Test
{
    [TestClass]
    public class SqlConnectionManagerTest
    {
        /// <summary>
        /// Test executing reader
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestReader()
        {
            const string stproc = "TestStproc";

            var reachedParams = false;
            var reachedReader = false;

            using (ShimsContext.Create())
            {
                ShimSqlConnection.ConstructorString = (conn, str) => { };
                ShimSqlConnection.AllInstances.DisposeBoolean = (conn, b) => { };
                ShimSqlConnection.AllInstances.OpenAsyncCancellationToken = (conn, token) => { return Task.FromResult(1); };
                ShimSqlConnection.AllInstances.CreateCommand = (conn) => { return new SqlCommand(); };

                ShimSqlCommand.AllInstances.CommandTextSetString = (command, str) =>
                {
                    Assert.AreEqual(stproc, str);
                };

                ShimSqlCommand.AllInstances.CommandTypeSetCommandType = (command, typ) =>
                {
                    Assert.AreEqual(CommandType.StoredProcedure, typ);
                };

                ShimSqlCommand.AllInstances.ExecuteReaderAsyncCancellationToken = (command, token) =>
                {
                    return Task.FromResult<SqlDataReader>(null);
                };

                await SqlConnectionManager.ExecuteSql(stproc, (paramCollection) =>
                {
                    reachedParams = true;
                    Assert.IsNotNull(paramCollection);
                },
                (reader) =>
                {
                    reachedReader = true;
                },
                CancellationToken.None);
            }

            Assert.IsTrue(reachedParams);
            Assert.IsTrue(reachedReader);
        }

        /// <summary>
        /// Test executing non query
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestNonQuery()
        {
            const string stproc = "TestStproc";

            var reachedParams = false;

            using (ShimsContext.Create())
            {
                ShimSqlConnection.ConstructorString = (conn, str) => { };
                ShimSqlConnection.AllInstances.DisposeBoolean = (conn, b) => { };
                ShimSqlConnection.AllInstances.OpenAsyncCancellationToken = (conn, token) => { return Task.FromResult(1); };
                ShimSqlConnection.AllInstances.CreateCommand = (conn) => { return new SqlCommand(); };

                ShimSqlCommand.AllInstances.CommandTextSetString = (command, str) =>
                {
                    Assert.AreEqual(stproc, str);
                };

                ShimSqlCommand.AllInstances.CommandTypeSetCommandType = (command, typ) =>
                {
                    Assert.AreEqual(CommandType.StoredProcedure, typ);
                };

                ShimSqlCommand.AllInstances.ExecuteNonQueryAsyncCancellationToken = (command, token) => Task.FromResult(1);

                await SqlConnectionManager.ExecuteSql(stproc, (paramCollection) =>
                {
                    reachedParams = true;
                    Assert.IsNotNull(paramCollection);
                },
                CancellationToken.None);
            }

            Assert.IsTrue(reachedParams);
        }
    }
}
