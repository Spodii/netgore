using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;

namespace GoreUpdater.Manager
{
    static class Program
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log.Info("Starting GoreUpdater server...");

            // ReSharper disable EmptyGeneralCatchClause
            // Create the root version directory
            try
            {
                var p = VersionHelper.GetVersionRootDir();
                if (!Directory.Exists(p))
                    Directory.CreateDirectory(p);
            }
            catch (Exception)
            {
            }
            // ReSharper restore EmptyGeneralCatchClause

            // ReSharper disable EmptyGeneralCatchClause
            // Ensure the directory for version 0 exists
            try
            {
                var p = VersionHelper.GetVersionPath(0);
                if (!Directory.Exists(p))
                    Directory.CreateDirectory(p);
            }
            catch (Exception)
            {
            }
            // ReSharper restore EmptyGeneralCatchClause

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}