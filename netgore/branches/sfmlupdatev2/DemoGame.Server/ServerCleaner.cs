using System;
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
    /// Performs cleaning operations on a <see cref="Server"/> - namely the database.
    /// </summary>
    public class ServerCleaner
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Server _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCleaner"/> class.
        /// </summary>
        /// <param name="server">The <see cref="Server"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="server"/> is null.</exception>
        public ServerCleaner(Server server)
        {
            if (server == null)
                throw new ArgumentNullException("server");

            _server = server;
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

        /// <summary>
        /// Runs the clean-up routines.
        /// </summary>
        /// <param name="extensive">If true, the extensive clean-up routines will also be run.
        /// This can take a long time, especially in larger databases.</param>
        public void Run(bool extensive = false)
        {
            if (log.IsInfoEnabled)
                log.Info("Starting cleanup");

            SetAccountCurrentIPsNull();

            if (extensive)
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
    }
}