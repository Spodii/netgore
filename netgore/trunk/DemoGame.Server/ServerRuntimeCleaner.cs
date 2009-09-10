using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DemoGame.Server.Queries;
using log4net;

namespace DemoGame.Server
{
    class ServerRuntimeCleaner
    {
        readonly Server _server;

        DBController DBController { get { return _server.DBController; } }

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ServerRuntimeCleaner(Server server)
        {
            if (server == null)
                throw new ArgumentNullException("server");

            _server = server;

            RunAll();
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
            DBController.GetQuery<SetAccountCurrentIPsNullQuery>().Execute();
        }

        static void LogCleanupRoutine(string description)
        {
            if (log.IsInfoEnabled)
                log.Info(" * " + description);
        }
    }
}
