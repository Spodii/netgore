using System;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Server.UI;

namespace DemoGame.Server
{
    class Program
    {
        /// <summary>
        /// Server program entry point.
        /// </summary>
        static void Main(string[] args)
        {
            Console.Title = "NetGore Server";

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