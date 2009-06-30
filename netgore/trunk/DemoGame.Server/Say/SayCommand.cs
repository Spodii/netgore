using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DemoGame.Server
{
    /// <summary>
    /// Describes a SayCommand.
    /// </summary>
    class SayCommand
    {
        readonly SayCommandCallback _callback;
        readonly bool _isThreadSafe;

        /// <summary>
        /// Gets the SayCommandCallback used to invoke this SayCommand.
        /// </summary>
        public SayCommandCallback Callback
        {
            get { return _callback; }
        }

        /// <summary>
        /// Gets if this SayCommand is thread-safe.
        /// </summary>
        public bool IsThreadSafe
        {
            get { return _isThreadSafe; }
        }

        public SayCommand(SayCommandCallback callback, bool isThreadsafe)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            _callback = callback;
            _isThreadSafe = isThreadsafe;
        }
    }
}