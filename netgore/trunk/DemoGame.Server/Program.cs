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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}