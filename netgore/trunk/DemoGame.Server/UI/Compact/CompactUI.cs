using System;
using System.Linq;
using System.Threading;

namespace DemoGame.Server.UI
{
    /// <summary>
    /// Provides a very compact interface to the server using the console.
    /// </summary>
    public class CompactUI : IDisposable
    {
        readonly Server _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompactUI"/> class.
        /// </summary>
        public CompactUI()
        {
            Console.WriteLine("NetGore Server - compact mode");

            // Create the server
            _server = new Server();
            _server.ConsoleCommandExecuted += Server_ConsoleCommandExecuted;

            Console.WriteLine("As long as this console remains open, the server will be running.");
            Console.WriteLine("No log messages will be printed to this screen, only manually-entered console commands.");
            Console.WriteLine("Type `help` to see the list of console commands.");

            // Spawn a thread to handle the console input
            var t = new Thread(ConsoleInputHandler) { IsBackground = true };
            t.Start();

            // Start the server loop, which will block the thread until the server closes
            _server.Start();
        }

        /// <summary>
        /// Method for the main server loop thread.
        /// </summary>
        void ConsoleInputHandler()
        {
            while (true)
            {
                // Read the line from the console
                var s = Console.ReadLine();
                if (string.IsNullOrEmpty(s))
                    continue;

                // Ensure the server is still running
                if (_server.IsDisposed)
                    break;

                // Forward the line to the server
                _server.EnqueueConsoleCommand(s);
            }
        }

        /// <summary>
        /// Handles when the server creates replies from console commands.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="command">The command.</param>
        /// <param name="returnString">The return string.</param>
        static void Server_ConsoleCommandExecuted(Server server, string command, string returnString)
        {
            Console.WriteLine(returnString);
            Console.WriteLine();
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _server.Dispose();
        }

        #endregion
    }
}