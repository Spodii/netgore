using System;
using System.IO;
using System.Windows.Forms;
using InstallationValidator.Tests;
using NetGore;

namespace InstallationValidator
{
    public class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}