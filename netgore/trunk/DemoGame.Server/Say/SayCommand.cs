using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    /// <summary>
    /// Describes a SayCommand.
    /// </summary>
    class SayCommand
    {
        readonly bool _isThreadSafe;
        readonly SayCommandCallback _callback;

        public SayCommand(SayCommandCallback callback, bool isThreadsafe)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            _callback = callback;
            _isThreadSafe = isThreadsafe;
        }

        /// <summary>
        /// Gets if this SayCommand is thread-safe.
        /// </summary>
        public bool IsThreadSafe { get { return _isThreadSafe; } }

        /// <summary>
        /// Gets the SayCommandCallback used to invoke this SayCommand.
        /// </summary>
        public SayCommandCallback Callback { get { return _callback; } }
    }
}
