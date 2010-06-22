using System;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using SFML.Graphics;

namespace InstallationValidator
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}