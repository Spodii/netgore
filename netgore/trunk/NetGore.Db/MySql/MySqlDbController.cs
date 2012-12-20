using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Db.MySql.QueryBuilder;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql
{
    /// <summary>
    /// A <see cref="DbControllerBase"/> for MySql.
    /// </summary>
    public class MySqlDbController : DbControllerBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbController"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MySqlDbController(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilder"/> to build queries for this connection.
        /// </summary>
        public override IQueryBuilder QueryBuilder
        {
            get { return MySqlQueryBuilder.Instance; }
        }

        /// <summary>
        /// When overridden in the derived class, creates a DbConnectionPool for this DbController.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>A DbConnectionPool for this DbController.</returns>
        protected override DbConnectionPool CreateConnectionPool(string connectionString)
        {
            return new MySqlDbConnectionPool(connectionString);
        }

        /// <summary>
        /// Gets an implementation of the <see cref="FindForeignKeysQuery"/> that works for this
        /// <see cref="DbControllerBase"/>.
        /// </summary>
        /// <param name="dbConnectionPool">The <see cref="DbConnectionPool"/> to use when creating the query.</param>
        /// <returns>The <see cref="FindForeignKeysQuery"/> to execute the query.</returns>
        protected override FindForeignKeysQuery GetFindForeignKeysQuery(DbConnectionPool dbConnectionPool)
        {
            return new MySqlFindForeignKeysQuery(dbConnectionPool);
        }

        /// <summary>
        /// Gets the SQL query string used for when testing if the database connection is valid. This string should
        /// be very simple and fool-proof, and work no matter what contents are in the database since this is just
        /// to test if the connection is working.
        /// </summary>
        /// <returns>The SQL query string used for when testing if the database connection is valid.</returns>
        protected override string GetTestQueryCommand()
        {
            return "SELECT 1+5";
        }

        /// <summary>
        /// When overridden in the derived class, removes all of the primary keys from a table where there is no foreign keys for the
        /// respective primary key.
        /// For safety reasons, if a column has no foreign keys, the query will be aborted.
        /// </summary>
        /// <param name="schema">The schema or database name of the table.</param>
        /// <param name="table">The table to check.</param>
        /// <param name="column">The primary key column.</param>
        /// <returns>The number of rows removed, or -1 if there were no foreign keys for the given column in the first place.</returns>
        public override int RemoveUnreferencedPrimaryKeys(string schema, string table, string column)
        {
            // How many keys to grab at a time. Larger value = greater memory usage, but fewer queries.
            const int batchSize = 4192;

            var ret = 0;

            // Get all of the fully-qualified foreign key columns
            var foreignKeys = GetPrimaryKeyReferences(schema, table, column);

            if (foreignKeys.IsEmpty())
            {
                const string errmsg = "Aborted RemoveUnreferencedPrimaryKeys on {0}.{1}.{2} - no foreign key references found.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, schema, table, column);
                return -1;
            }

            // Grab a raw database connection so we can execute queries directly
            using (var pconn = ConnectionPool.Acquire())
            {
                var conn = pconn.Connection;

                var fkCmds = new List<DbCommand>();
                DbCommand grabPKsCmd = null;
                DbCommand deleteCmd = null;

                // Find the number of rows in the table
                int numRows;
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT COUNT(*) FROM {0}.{1}", schema, table);
                    using (var r = cmd.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        if (!r.Read())
                        {
                            const string errmsg = "Failed to read result for query: {0}";
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, cmd.CommandText);
                            Debug.Fail(string.Format(errmsg, cmd.CommandText));
                            return 0;
                        }

                        numRows = r.GetInt32(0);
                    }
                }

                if (numRows == 0)
                    return 0;

                try
                {
                    /* CREATE THE COMMANDS */

                    // Create the command to grab the primary keys
                    grabPKsCmd = conn.CreateCommand();
                    grabPKsCmd.CommandText = string.Format("SELECT `{0}` FROM {1}.{2} LIMIT @low, @high", column, schema, table);

                    var grabPKsParamLow = grabPKsCmd.CreateParameter();
                    grabPKsParamLow.ParameterName = "@low";
                    grabPKsCmd.Parameters.Add(grabPKsParamLow);

                    var grabPKsParamHigh = grabPKsCmd.CreateParameter();
                    grabPKsParamHigh.ParameterName = "@high";
                    grabPKsCmd.Parameters.Add(grabPKsParamHigh);

                    // Create the command for deleting the row
                    deleteCmd = conn.CreateCommand();
                    deleteCmd.CommandText = string.Format("DELETE FROM {0}.{1} WHERE `{2}` = @value", schema, table, column);

                    var deleteParam = deleteCmd.CreateParameter();
                    deleteParam.ParameterName = "@value";
                    deleteCmd.Parameters.Add(deleteParam);

                    // Create a command for each of the individual foreign key references
                    foreach (var fk in foreignKeys)
                    {
                        var cmd = conn.CreateCommand();
                        cmd.CommandText = string.Format("SELECT COUNT(*) FROM {0}.{1} WHERE `{2}`=@value", fk.Schema, fk.Table,
                            fk.Column);

                        var p = cmd.CreateParameter();
                        p.ParameterName = "@value";
                        cmd.Parameters.Add(p);

                        fkCmds.Add(cmd);
                    }

                    /* SEARCH AND DESTROY */

                    // Loop as many times as we need for each batch to run
                    var rowsLow = 0;
                    while (rowsLow < numRows)
                    {
                        // Grab all the primary key values for this batch
                        var primaryKeys = new List<object>();
                        grabPKsCmd.Parameters["@low"].Value = rowsLow;
                        grabPKsCmd.Parameters["@high"].Value = batchSize;

                        using (var r = grabPKsCmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                primaryKeys.Add(r[0]);
                            }
                        }

                        // Loop though each primary key value
                        foreach (var pk in primaryKeys)
                        {
                            var pkInUse = false;

                            // Loop through each queries for the foreign keys
                            foreach (var cmd in fkCmds)
                            {
                                // Set the value to look for equal to that of our primary key
                                cmd.Parameters[0].Value = pk;

                                // Execute the query to see if there is any usages of the primary key
                                using (var r = cmd.ExecuteReader(CommandBehavior.SingleResult))
                                {
                                    if (!r.Read())
                                    {
                                        const string errmsg = "Failed to read result for query: {0}";
                                        if (log.IsErrorEnabled)
                                            log.ErrorFormat(errmsg, cmd.CommandText);
                                        Debug.Fail(string.Format(errmsg, cmd.CommandText));

                                        // When we fail to read the result, completely skip this value just in case
                                        // there is something wrong. Safer to not delete than to delete.
                                        pkInUse = true;
                                        break;
                                    }

                                    var count = r.GetInt32(0);
                                    if (count > 0)
                                        pkInUse = true;
                                }

                                if (pkInUse)
                                    break;
                            }

                            // If the primary key is not in use, delete it
                            if (!pkInUse)
                            {
                                const string msg = "Deleting pk `{0}` from {1}.{2}.";
                                if (log.IsDebugEnabled)
                                    log.DebugFormat(msg, pk, schema, table);

                                deleteCmd.Parameters[0].Value = pk;
                                var rowsDeleted = deleteCmd.ExecuteNonQuery();
                                Debug.Assert(rowsDeleted == 0 || rowsDeleted == 1);

                                ret += rowsDeleted;

                                // Adjust the row count since we did just delete a row
                                numRows -= rowsDeleted;
                                rowsLow -= rowsDeleted;
                            }
                        }

                        // Move down to the next batch
                        rowsLow += batchSize;
                    }
                }
                finally
                {
                    // Release the commands
                    if (deleteCmd != null)
                        deleteCmd.Dispose();

                    if (grabPKsCmd != null)
                        grabPKsCmd.Dispose();

                    foreach (var cmd in fkCmds)
                    {
                        cmd.Dispose();
                    }
                }
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Deleted {0} unreferenced rows from {1}.{2} using column {3}.", ret, schema, table, column);

            return ret;
        }


        /// <summary>
        /// Executes raw sql directly against the database with no parameterization or pooling.
        /// Only use this for quick one-off queries, such as queries to run during load/unload of the server or editor-specific tasks.
        /// </summary>
        /// <param name="sql">The sql to execute.</param>
        public override void ExecuteNonQuery(string sql)
        {
            using (var conn = ConnectionPool.Acquire())
            {
                using (var trans = conn.Connection.BeginTransaction())
                {
                    using (var dbCommand = conn.Connection.CreateCommand())
                    {
                        dbCommand.CommandText = sql;
                        dbCommand.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
            }
        }

        /// <summary>
        /// Executes raw sql directly against the database with no parameterization or pooling.
        /// Only use this for quick one-off queries, such as queries to run during load/unload of the server or editor-specific tasks.
        /// </summary>
        /// <param name="sqls">The sql queries to execute. Gets wrapped up in a transaction.</param>
        public override void ExecuteNonQueries(params string[] sqls)
        {
            using (var conn = ConnectionPool.Acquire())
            {
                using (var trans = conn.Connection.BeginTransaction())
                {
                    try
                    {
                        using (var dbCommand = conn.Connection.CreateCommand())
                        {
                            dbCommand.Connection = conn.Connection;
                            dbCommand.Transaction = trans;
                            foreach (string sql in sqls)
                            {
                                dbCommand.CommandText = sql;
                                dbCommand.ExecuteNonQuery();
                            }
                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Executes raw sql directly against the database with no parameterization or pooling.
        /// Only use this for quick one-off queries, such as queries to run during load/unload of the server or editor-specific tasks.
        /// </summary>
        /// <param name="sql">The sql to execute.</param>
        /// <param name="readFunc">The function used to describe how to the results of each row.</param>
        /// <typeparam name="T">The return type for read rows.</typeparam>
        public override List<T> ExecuteQuery<T>(string sql, Func<DbDataReader, T> readFunc)
        {
            List<T> ret = new List<T>();

            using (var conn = ConnectionPool.Acquire())
            {
                using (var dbCommand = conn.Connection.CreateCommand())
                {
                    dbCommand.CommandText = sql;
                    using (var r = dbCommand.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            ret.Add(readFunc(r));
                        }
                    }
                }
            }

            return ret;
        }
    }
}