using System;
using System.ComponentModel;
using System.Windows.Forms;
using DemoGame.Server;

namespace DemoGame.MapEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ScreenForm());
        }
    }
}