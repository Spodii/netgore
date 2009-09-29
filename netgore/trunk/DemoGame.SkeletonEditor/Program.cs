using System;
using System.Linq;
using System.Windows.Forms;
using NetGore;

namespace DemoGame.SkeletonEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var switches = CommandLineSwitchHelper.GetCommandsUsingEnum<CommandLineSwitch>(args).ToArray();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ScreenForm(switches));
        }
    }
}