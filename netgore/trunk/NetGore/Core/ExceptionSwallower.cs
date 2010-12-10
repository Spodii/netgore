using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;

namespace NetGore
{
    /// <summary>
    /// Handles <see cref="Exception"/> that have managed to propogate to the top level of the application. This should only
    /// be used for when swallowing <see cref="Exception"/>s at the highest level, not for catching <see cref="Exception"/>s
    /// just to ignore them.
    /// This is primarily intended to be used in WinForm applications to prevent an unhandled <see cref="Exception"/> from
    /// propagating high enough to break the application.
    /// </summary>
    public class ExceptionSwallower
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static int _exceptionsSwallowed = 0;
        static ExceptionSwallower _instance;

        readonly bool _rethrow;

        /// <summary>
        /// Initializes the <see cref="ExceptionSwallower"/> class.
        /// </summary>
        static ExceptionSwallower()
        {
            // Set the default ExceptionSwallower instance
            _instance = new ExceptionSwallower();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionSwallower"/> class.
        /// </summary>
        /// <param name="rethrow">If <see cref="Exception"/>s should be rethrown</param>
        public ExceptionSwallower(bool rethrow = true)
        {
            _rethrow = rethrow;
        }

        /// <summary>
        /// Gets the total number of <see cref="Exception"/>s that have been swallowed since this application has been running.
        /// </summary>
        public static int ExceptionsSwallowed
        {
            get { return _exceptionsSwallowed; }
        }

        /// <summary>
        /// Gets the global <see cref="ExceptionSwallower"/> instance.
        /// </summary>
        public static ExceptionSwallower Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets if <see cref="Exception"/>s should be re-thrown after being swallowed.
        /// </summary>
        /// <example>
        /// try {
        ///     /* Perform task */
        /// } catch (Exception ex) {
        ///     ExceptionSwallower.Instance.Swallow(ex);
        ///     if (ExceptionSwallower.Instance.Rethrow)
        ///         throw;
        /// }
        /// </example>
        public bool Rethrow
        {
            get { return _rethrow; }
        }

        /// <summary>
        /// Allows for general handling of an unhandled <see cref="Exception"/>. The passed <see cref="Exception"/> will not be treated
        /// (as that logic should be done in a normal try/catch block). Instead, the <see cref="Exception"/> will be logged so that
        /// its existance is not completely forgotten.
        /// </summary>
        /// <param name="ex">The swallowed unhandled <see cref="Exception"/>.</param>
        public void Swallow(Exception ex)
        {
            Interlocked.Increment(ref _exceptionsSwallowed);

            SwallowHandler(ex);
        }

        /// <summary>
        /// Performs the actual swallowing of the unhandled <see cref="Exception"/>. Derived classes should override this method to implement
        /// their own handling logic.
        /// </summary>
        /// <param name="ex">The swallowed unhandled <see cref="Exception"/>.</param>
        protected virtual void SwallowHandler(Exception ex)
        {
            const string errmsg = "`{0}` swallowed unhandled exception: {1}";

            if (log.IsFatalEnabled)
                log.FatalFormat(errmsg, this, ex);

            Debug.Fail(string.Format(errmsg, this, ex));
        }

        /// <summary>
        /// Attempts to change the <see cref="ExceptionSwallower"/> instance.
        /// </summary>
        /// <param name="newInstance">The new <see cref="ExceptionSwallower"/> instance.</param>
        /// <returns>True if the <paramref name="newInstance"/> was set as the new global <see cref="ExceptionSwallower"/>
        /// instance; otherwise false.</returns>
        public static bool TrySetInstance(ExceptionSwallower newInstance)
        {
            if (newInstance == null)
                return false;

            _instance = newInstance;

            return true;
        }
    }
}