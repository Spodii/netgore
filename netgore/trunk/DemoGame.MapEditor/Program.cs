using System;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.EditorTools;

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
#if DEBUG
            WinFormExceptionHelper.AddUnhandledExceptionHooks();
#endif

            var switches = CommandLineSwitchHelper.GetCommandsUsingEnum<CommandLineSwitch>(args).ToArray();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ScreenForm(switches));
        }
    }
}