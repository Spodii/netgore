using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NetGore
{
    /// <summary>
    /// A thread-safe buffer that reads the input from the console and buffers it until it is ready to be handled.
    /// </summary>
    public class ConsoleInputBuffer : IDisposable
    {
        static readonly IEnumerable<string> _emptyStrings = Enumerable.Empty<string>();

        readonly Queue<string> _buffer = new Queue<string>();
        readonly object _syncObj = new object();
        bool _isRunning = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleInputBuffer"/> class.
        /// </summary>
        public ConsoleInputBuffer()
        {
            var inputThread = new Thread(ReceiveInputLoop) { IsBackground = true, Name = "Console Input" };
            inputThread.Start();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            _isRunning = false;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ConsoleInputBuffer"/> is reclaimed by garbage collection.
        /// </summary>
        ~ConsoleInputBuffer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets an IEnumerable of the input strings since the last call to this method.
        /// </summary>
        /// <returns>An IEnumerable of the input strings since the last call to this method.</returns>
        public IEnumerable<string> GetBuffer()
        {
            if (_buffer.Count == 0)
                return _emptyStrings;

            IEnumerable<string> ret;
            lock (_syncObj)
            {
                if (_buffer.Count == 0)
                    ret = _emptyStrings;
                else
                {
                    ret = _buffer.ToArray();
                    _buffer.Clear();
                }
            }

            return ret;
        }

        /// <summary>
        /// The thread that does the actual reading from the console.
        /// </summary>
        void ReceiveInputLoop()
        {
            while (_isRunning)
            {
                var input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    lock (_syncObj)
                    {
                        _buffer.Enqueue(input);
                    }
                }
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_isRunning)
                return;

            GC.SuppressFinalize(this);

            Dispose(true);
        }

        #endregion
    }
}