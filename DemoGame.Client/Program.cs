using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Main application entry point - does nothing more than start the primary class
    /// </summary>
    static class Program
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

        static void Main()
        {
            log.Info("Starting client...");

            ThreadAsserts.IsMainThread();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Try to copy dev content over
            CopyContent();

            // Start the game
            using (var game = new DemoGame())
            {
                game.Run();
            }
        }
    }
}