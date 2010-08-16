using System;
using System.Diagnostics;
using System.Reflection;
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
        static ExceptionSwallower _instance;

        /// <summary>
        /// Gets the global <see cref="ExceptionSwallower"/> instance.
        /// </summary>
        public static ExceptionSwallower Instance { get { return _instance; } }

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

        /// <summary>
        /// Initializes the <see cref="ExceptionSwallower"/> class.
        /// </summary>
        static ExceptionSwallower()
        {
            // Set the default ExceptionSwallower instance
            _instance = new ExceptionSwallower(false);
        }

        /// <summary>
        /// Gets if <see cref="Exception"/>s should be re-thrown after being swallowed. This will likely always be false, but support
        /// for rethrowing (using "throw;" inside of an exception block) should still be supported.
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
        public bool Rethrow { get { return _rethrow; } }

        readonly bool _rethrow;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionSwallower"/> class.
        /// </summary>
        /// <param name="rethrow">If <see cref="Exception"/>s should be rethrown. Default and recommended value is false.</param>
        public ExceptionSwallower(bool rethrow = false)
        {
            _rethrow = rethrow;
        }

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Allows for general handling of an unhandled <see cref="Exception"/>. The passed <see cref="Exception"/> will not be treated
        /// (as that logic should be done in a normal try/catch block). Instead, the <see cref="Exception"/> will be logged so that
        /// its existance is not completely forgotten.
        /// </summary>
        /// <param name="ex">The unhandled <see cref="Exception"/>.</param>
        public virtual void Swallow(Exception ex)
        {
            const string errmsg = "`{0}` swallowed unhandled exception: {1}";

            if (log.IsFatalEnabled)
                log.FatalFormat(errmsg, this, ex);

            Debug.Fail(string.Format(errmsg, this, ex));
        }
    }
}