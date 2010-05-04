using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore;
using NetGore.EditorTools;
using NetGore.IO;

namespace DemoGame.MapEditor
{
    static class Program
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            log.Info("Starting map editor...");

#if DEBUG
            WinFormExceptionHelper.AddUnhandledExceptionHooks();
#endif

            EngineSettingsInitializer.Initialize();
            var switches = CommandLineSwitchHelper.GetCommandsUsingEnum<CommandLineSwitch>(args).ToArray();

            // Ensure the content is copied over
            if (!ContentPaths.TryCopyContent())
            {
                const string errmsg =
                    "Failed to copy the content from the dev to build path." +
                    " Content in the build path will likely not update to reflect changes made in the content in the dev path.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg);
                Debug.Fail(errmsg);
            }

            // Run the program
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ScreenForm(switches));
        }
    }
}