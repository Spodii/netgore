using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// DBProviderFactory implementation for MysqlClient.
    /// </summary>
    public sealed class MySqlClientFactory : DbProviderFactory
    {
        /// <summary>
        /// Gets an instance of the <see cref="MySqlClientFactory"/>. 
        /// This can be used to retrieve strongly typed data objects. 
        /// </summary>
        public static readonly MySqlClientFactory Instance;

        /// <summary>
        /// Returns true if a <b>MySqlDataSourceEnumerator</b> can be created; 
        /// otherwise false. 
        /// </summary>
        public override bool CanCreateDataSourceEnumerator
        {
            get { return false; }
        }

        static MySqlClientFactory()
        {
            Instance = new MySqlClientFactory();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbCommand"/> instance. 
        /// </summary>
        /// <returns>A new strongly typed instance of <b>DbCommand</b>.</returns>
        public override DbCommand CreateCommand()
        {
            return new MySqlCommand();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbCommandBuilder"/> instance. 
        /// </summary>
        /// <returns>A new strongly typed instance of <b>DbCommandBuilder</b>.</returns>
        public override DbCommandBuilder CreateCommandBuilder()
        {
            return new MySqlCommandBuilder();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbConnection"/> instance. 
        /// </summary>
        /// <returns>A new strongly typed instance of <b>DbConnection</b>.</returns>
        public override DbConnection CreateConnection()
        {
            return new MySqlConnection();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbConnectionStringBuilder"/> instance. 
        /// </summary>
        /// <returns>A new strongly typed instance of <b>DbConnectionStringBuilder</b>.</returns>
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new MySqlConnectionStringBuilder();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbDataAdapter"/> instance. 
        /// </summary>
        /// <returns>A new strongly typed instance of <b>DbDataAdapter</b>. </returns>
        public override DbDataAdapter CreateDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        /// <summary>
        /// Returns a strongly typed <see cref="DbParameter"/> instance. 
        /// </summary>
        /// <returns>A new strongly typed instance of <b>DbParameter</b>.</returns>
        public override DbParameter CreateParameter()
        {
            return new MySqlParameter();
        }
    }
}