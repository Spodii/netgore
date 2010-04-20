using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NetGore
{
    /// <summary>
    /// Provides threading-related asserts.
    /// </summary>
    public static class ThreadAsserts
    {
        static Thread _mainThread;

        /// <summary>
        /// Checks if the current thread calling this method is part of the main thread. The main thread is determined
        /// by the thread that first calls this method.
        /// </summary>
        [Conditional("DEBUG")]
        public static void IsMainThread()
        {
            var t = Thread.CurrentThread;

            if (_mainThread == null)
                _mainThread = t;
            else if (_mainThread != t)
                Debug.Fail("This method was not called from the main thread!");
        }
    }
}