using System;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server
{
    class ServerRuntimeCleaner
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly Server _server;

        public ServerRuntimeCleaner(Server server)
        {
            if (server == null)
                throw new ArgumentNullException("server");

            _server = server;

            RunAll();
        }

        IDbController DbController
        {
            get { return _server.DbController; }
        }

        static void LogCleanupRoutine(string description)
        {
            if (log.IsInfoEnabled)
                log.Info(" * " + description);
        }

        void RunAll()
        {
            if (log.IsInfoEnabled)
                log.Info("Starting cleanup");

            SetAccountCurrentIPsNull();

            if (log.IsInfoEnabled)
                log.Info("Cleanup complete");
        }

        void SetAccountCurrentIPsNull()
        {
            LogCleanupRoutine("Setting all current_ip values in account table to null");

            // Set the current_ip on all accounts null
            DbController.GetQuery<SetAccountCurrentIPsNullQuery>().Execute();
        }
    }
}