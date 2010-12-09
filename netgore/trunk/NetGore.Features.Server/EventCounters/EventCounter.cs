using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using log4net;
using NetGore.Db;

namespace NetGore.Features.EventCounters
{
    /// <summary>
    /// An object that is able to update a collection of event counters.
    /// </summary>
    /// <typeparam name="TObjectID">The type of object ID.</typeparam>
    /// <typeparam name="TEventID">The type of event ID.</typeparam>
    public class EventCounter<TObjectID, TEventID>:IEventCounter<TObjectID, TEventID>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IDbQueryNonReader<ObjectEventAmount<TObjectID, TEventID>> _query;
        readonly object _updateCacheSync = new object();
        readonly Timer _flushTimer;

        Dictionary<Tuple<TObjectID, TEventID>, int> _updateCache = new Dictionary<Tuple<TObjectID, TEventID>, int>();

        /// <summary>
        /// Object used to synchronize flushing.
        /// </summary>
        readonly object _flushSync = new object();

        /// <summary>
        /// The rate, in milliseconds, that the cache will be flushed.
        /// </summary>
        const int _flushRate = 1000 * 60 * 5; // 5 minutes

        public EventCounter(IDbQueryNonReader<ObjectEventAmount<TObjectID, TEventID>> query)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            _query = query;

            _flushTimer = new Timer(FlushTimerCallback, null, _flushRate, _flushRate);
        }

        /// <summary>
        /// Callback for the <see cref="_flushTimer"/>.
        /// </summary>
        /// <param name="state">The state object. Unused.</param>
        void FlushTimerCallback(object state)
        {
            Flush();
        }

        /// <summary>
        /// Increments the counter.
        /// </summary>
        /// <param name="source">The source object to increment the counter for.</param>
        /// <param name="e">The event to increment.</param>
        /// <exception cref="ObjectDisposedException">This object has been disposed.</exception>
        public void Increment(TObjectID source, TEventID e)
        {
            Increment(source, e, 1);
        }

        /// <summary>
        /// Increments the counter.
        /// </summary>
        /// <param name="source">The source object to increment the counter for.</param>
        /// <param name="e">The event to increment.</param>
        /// <param name="amount">The amount to increment.</param>
        /// <exception cref="ObjectDisposedException">This object has been disposed.</exception>
        public void Increment(TObjectID source, TEventID e, int amount)
        {
            // Create the key
            var key = new Tuple<TObjectID, TEventID>(source, e);

            int flushValue = 0;

            lock (_updateCacheSync)
            {
                bool mustAddValue = false;

                // Grab the value from the cache
                int value;
                if (!_updateCache.TryGetValue(key, out value))
                {
                    // Value wasn't in the cache
                    mustAddValue = true;
                    value = 0;
                }

                try
                {
                    // Update the value
                    checked
                    {
                        value += amount;
                    }
                }
                catch (OverflowException ex)
                {
                    const string errmsg = "Overflow occured for source `{0}`, event `{1}`. Flushing buffer to resolve issue," + 
                                          " though it might be suspicious that the value managed to get so large. Exception: {2}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, source, e, ex);

                    // Set the value the flush
                    flushValue = value;

                    // Set the cache value to the amount
                    value = amount;
                }

                // Add the value back the the cache
                if (!mustAddValue)
                {
                    // Update
                    _updateCache[key] = value;
                }
                else
                {
                    // Add
                    _updateCache.Add(key, value);
                }
            }
            
            // If we have the flushValue set, then flush it (doing it here instead of above so we hold onto the
            // lock for as short of time as possible)
            if (flushValue != 0)
                WriteValue(source, e, flushValue);
        }

        /// <summary>
        /// Forces the internal cache to be synchronously flushed and executes the queries against the database.
        /// </summary>
        public void Flush()
        {
            lock (_flushSync)
            {
                // Check if we need to flush
                lock (_updateCacheSync)
                {
                    if (_updateCache.Count == 0)
                        return;
                }

                // To very quickly swap out the cache with a new one so we can work on it without a lock, we create a new
                // instance and just swap the references while we have the lock
                var cache = new Dictionary<Tuple<TObjectID, TEventID>, int>();
                lock (_updateCacheSync)
                {
                    // Perform the swap
                    var swap = _updateCache;
                    _updateCache = cache;
                    cache = swap;

                    Debug.Assert(_updateCache.Count == 0);
                    Debug.Assert(cache.Count != 0);
                }

                // Flush out every value
                Flush(cache);
            }
        }

        /// <summary>
        /// Flushes all of the items in the cache.
        /// </summary>
        /// <param name="values">The collection containing the values to flush.</param>
        void Flush(IEnumerable<KeyValuePair<Tuple<TObjectID, TEventID>, int>> values)
        {
            foreach (var v in values)
            {
                var value = new ObjectEventAmount<TObjectID, TEventID>(v.Key.Item1, v.Key.Item2, v.Value);
                WriteValue(value);
            }
        }

        /// <summary>
        /// Writes a single counter value to the database.
        /// </summary>
        /// <param name="source">The source object to increment the counter for.</param>
        /// <param name="e">The event to increment.</param>
        /// <param name="amount">The amount to increment.</param>
        void WriteValue(TObjectID source, TEventID e, int amount)
        {
            var value = new ObjectEventAmount<TObjectID, TEventID>(source, e, amount);
            WriteValue(value);
        }

        /// <summary>
        /// Writes a single value to the database.
        /// </summary>
        /// <param name="value">The <see cref="ObjectEventAmount{T,U}"/>.</param>
        void WriteValue(ObjectEventAmount<TObjectID, TEventID> value)
        {
            _query.Execute(value);
        }

        bool _isDisposed = false;

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        public bool IsDisposed { get { return _isDisposed; } }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="EventCounter{TObjectID, TEventID}"/> is reclaimed by garbage collection.
        /// </summary>
        ~EventCounter()
        {
            Debug.Assert(!IsDisposed, "How was this disposed but the destructor called?");

            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources; <c>false</c>
        /// to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            // Dispose of the timer
            try
            {
                if (_flushTimer != null)
                    _flushTimer.Dispose();
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to dispose object `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, _flushTimer, ex);
                Debug.Fail(string.Format(errmsg, _flushTimer, ex));
            }

            // Perform one last flush
            try
            {
                Flush();
            }
            catch (Exception ex)
            {
                const string errmsg = "Flush failed. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            _isDisposed = true;

            GC.SuppressFinalize(this);

            Dispose(true);
        }
    }
}