using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class InterfaceTests : BaseTest
    {
        [Test]
        public void ClientFactory()
        {
            DbProviderFactory f = new MySqlClientFactory();
            DbConnection c = f.CreateConnection();
            DbConnectionStringBuilder cb = f.CreateConnectionStringBuilder();
            cb.ConnectionString = GetConnectionString(true);
            c.ConnectionString = cb.ConnectionString;
            c.Open();

            DbCommand cmd = f.CreateCommand();
            cmd.Connection = c;
            cmd.CommandText = "SHOW TABLES FROM test";
            cmd.CommandType = CommandType.Text;
            DbDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}