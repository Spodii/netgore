using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}