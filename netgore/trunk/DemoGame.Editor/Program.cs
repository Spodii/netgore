using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DemoGame.Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // NOTE: Forces SFML to load early on instead of later, causing a LoaderLock exception
            using (new SFML.Graphics.Image())
            {
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
