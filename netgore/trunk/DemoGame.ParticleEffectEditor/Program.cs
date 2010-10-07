using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.Editor.WinForms;

namespace DemoGame.ParticleEffectEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            WinFormExceptionHelper.AddUnhandledExceptionHooks();
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}