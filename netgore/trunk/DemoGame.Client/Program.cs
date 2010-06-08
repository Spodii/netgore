using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore;
using NetGore.Features.DisplayAction;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Main application entry point - does nothing more than start the primary class
    /// </summary>
    static class Program
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            log.Info("Starting client...");

            ThreadAsserts.IsMainThread();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameForm());
        }
    }
}