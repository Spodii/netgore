using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore.Db;

namespace DemoGame.Server
{
    /// <summary>
    /// Performs cleaning operations on a <see cref="Server"/> database.
    /// </summary>
    class ServerRuntimeCleaner
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly Server _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerRuntimeCleaner"/> class.
        /// </summary>
        /// <param name="server">The <see cref="Server"/> instance.</param>
        public ServerRuntimeCleaner(Server server)
        {
            if (server == null)
                throw new ArgumentNullException("server");

            _server = server;

            RunAll();
        }

        /// <summary>
        /// Gets the <see cref="IDbController"/>.
        /// </summary>
        IDbController DbController
        {
            get { return _server.DbController; }
        }

        /// <summary>
        /// Writes to the clean-up log.
        /// </summary>
        /// <param name="description">The log entry.</param>
        static void LogCleanupRoutine(string description)
        {
            if (log.IsInfoEnabled)
                log.Info(" * " + description);
        }

        /// <summary>
        /// Runs all of the clean-up operations.
        /// </summary>
        void RunAll()
        {
            if (log.IsInfoEnabled)
                log.Info("Starting cleanup");

            SetAccountCurrentIPsNull();
            RemoveUnreferencedItems();

            if (log.IsInfoEnabled)
                log.Info("Cleanup complete");
        }

        /// <summary>
        /// Sets the current IP on all accounts to null.
        /// </summary>
        void SetAccountCurrentIPsNull()
        {
            LogCleanupRoutine("Setting all current_ip values in account table to null");

            // Set the current_ip on all accounts null
            DbController.GetQuery<SetAccountCurrentIPsNullQuery>().Execute();
        }

        /// <summary>
        /// Removes all entries from the items table that has no foreign key references.
        /// </summary>
        void RemoveUnreferencedItems()
        {
            LogCleanupRoutine("Removing ununused item instances");

            if (ItemTable.DbKeyColumns.Count() != 1)
            {
                const string errmsg = "Cannot execute cleanup routine - expected only one primary key column.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            DbController.RemoveUnreferencedPrimaryKeys(DbController.Database, ItemTable.TableName, ItemTable.DbKeyColumns.First());
        }
    }
}