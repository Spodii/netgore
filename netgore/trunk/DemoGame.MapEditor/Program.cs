using System;
using System.Linq;
using System.Windows.Forms;
using NetGore;

namespace DemoGame.MapEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            args = new string[] { "--saveallmaps", "--close" };
            var switches = CommandLineSwitchHelper.GetCommandsUsingEnum<CommandLineSwitch>(args);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ScreenForm(switches));
        }
    }
}