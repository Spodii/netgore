using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Provides helper methods for handling <see cref="Exception"/>s in WinForm applications.
    /// </summary>
    public static class WinFormExceptionHelper
    {
        const string _exceptionMessage = "Unhandled exception caught: ";

        /// <summary>
        /// Creates hooks that will listen for any unhandled <see cref="Exception"/>s.
        /// </summary>
        public static void AddUnhandledExceptionHooks()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Debug.Fail(_exceptionMessage + e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.Fail(_exceptionMessage + e.ExceptionObject);
        }
    }
}
