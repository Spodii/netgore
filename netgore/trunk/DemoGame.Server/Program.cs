using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;

namespace DemoGame.Server
{
    class Program
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static Server _server;
        static ConsoleCtrlHandler _handler;

        delegate bool ConsoleCtrlHandler(CtrlTypes ctrlType);

        static void Main()
        {
            if (log.IsInfoEnabled)
                log.Info("Starting server...");

            // Hack in a callback that will allow us to dispose of the server if the console is closed in any way
            // Ideally, people would type "quit" or "close" when they want to close it, but hell, not even I do that
            _handler += ConsoleCtrlCheck;
            SetConsoleCtrlHandler(_handler, true);

            // Create and start the server
            _server = new Server();
            _server.Start();
            _server.Dispose();
        }

        static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            switch (ctrlType)
            {
                case CtrlTypes.CTRL_C_EVENT:
                case CtrlTypes.CTRL_BREAK_EVENT:
                case CtrlTypes.CTRL_CLOSE_EVENT:
                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    if (_server != null && !_server.IsDisposed)
                        _server.Dispose();
                    break;
            }

            return true;
        }

        [DllImport("Kernel32")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandler handler, bool add);

        enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }
    }
}