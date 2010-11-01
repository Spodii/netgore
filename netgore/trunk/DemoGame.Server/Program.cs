using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DemoGame.Server.UI;
using log4net;
using NetGore.IO;

namespace DemoGame.Server
{
    class Program
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Calls <see cref="ContentPaths.TryCopyContent"/> in debug builds.
        /// </summary>
        [Conditional("DEBUG")]
        static void CopyContent()
        {
            if (ContentPaths.TryCopyContent(userArgs: CommonConfig.TryCopyContentArgs))
            {
                if (log.IsInfoEnabled)
                    log.Info("TryCopyContent succeeded");
            }
            else
            {
                if (log.IsInfoEnabled)
                    log.Info("TryCopyContent failed");
            }
        }

        /// <summary>
        /// Server program entry point.
        /// </summary>
        static void Main(string[] args)
        {
            log.Info("Starting server...");

            Console.Title = "NetGore Server";

            // Copy content
            CopyContent();

            // Check to run in compact mode
            if (args != null && args.Any(x => StringComparer.OrdinalIgnoreCase.Equals(x, "--compact")))
            {
                using (new CompactUI())
                {
                }
            }
            else
            {
                // Run in WinForms mode by default
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
        }
    }
}