using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using log4net;

namespace DemoGame.Server.UI
{
    public abstract partial class ConsoleWindowHider
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The Windows implementation of <see cref="ConsoleWindowHider"/>.
        /// </summary>
        class WindowsConsoleWindowHider : ConsoleWindowHider
        {
#if !MONO
            [DllImport("user32.dll")]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll")]
            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
#endif

            /// <summary>
            /// When overridden in the derived class, hides the console window.
            /// </summary>
            protected override void DoHide()
            {
#if !MONO
                if (log.IsDebugEnabled)
                    log.Debug("Hiding console window");

                // Hide the console window
                var ptr = FindWindow(null, Console.Title);
                ShowWindow(ptr, 0);

#else

                const string errmsg = "This implementation is only available in non-Mono builds!";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                throw new InvalidOperationException(errmsg);
#endif
            }
        }
    }
}