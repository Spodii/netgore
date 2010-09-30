using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore;
using NetGore.IO;

namespace DemoGame.Editor
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
            log.Info("Starting editor...");

            // NOTE: Forces SFML to load early on instead of later, causing a LoaderLock exception. Can probably be removed later.
            using (new SFML.Graphics.Image())
            {
            }

            // Initialize stuff
            EngineSettingsInitializer.Initialize();
            GlobalState.Initailize();

            // Get the command-line switches
            var switches = CommandLineSwitchHelper.GetCommandsUsingEnum<CommandLineSwitch>(args).ToArray();

            // Ensure the content is copied over
            if (!ContentPaths.TryCopyContent(userArgs: "--clean=\"[Engine,Font,Fx,Grh,Languages,Maps,Music,Skeletons,Sounds]\""))
            {
                const string errmsg =
                    "Failed to copy the content from the dev to build path." +
                    " Content in the build path will likely not update to reflect changes made in the content in the dev path.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg);
                Debug.Fail(errmsg);
            }

            // Start up the application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
