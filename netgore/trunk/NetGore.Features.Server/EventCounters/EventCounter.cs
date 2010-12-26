using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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
    public class EventCounter<TObjectID, TEventID> : IEventCounter<TObjectID, TEventID>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The rate, in milliseconds, that the cache will be flushed.
        /// </summary>
        const int _flushRate = 1000 * 60 * 5; // 5 minutes

        /// <summary>
        /// The <see cref="CacheKeyComparer"/> instance.
        /// </summary>
        static readonly CacheKeyComparer _keyComparer = new CacheKeyComparer();

        /// <summary>
        /// Object used to synchronize flushing.
        /// </summary>
        readonly object _flushSync = new object();

        readonly Timer _flushTimer;

        readonly IDbQueryNonReader<ObjectEventAmount<TObjectID, TEventID>> _query;
        readonly object _updateCacheSync = new object();

        bool _isDisposed = false;
        Dictionary<CacheKey, int> _updateCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCounter{TObjectID, TEventID}"/> class.
        /// </summary>
        /// <param name="query">The query to use to perform updates.</param>
        /// <exception cref="ArgumentNullException"><paramref name="query" /> is <c>null</c>.</exception>
        public EventCounter(IDbQueryNonReader<ObjectEventAmount<TObjectID, TEventID>> query)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            _query = query;
            _updateCache = CreateUpdateCache();

            _flushTimer = new Timer(FlushTimerCallback, null, _flushRate, _flushRate);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{T,U}"/> instance for the update cache.
        /// </summary>
        /// <returns>A <see cref="Dictionary{T,U}"/> instance for the update cache.</returns>
        static Dictionary<CacheKey, int> CreateUpdateCache()
        {
            return new Dictionary<CacheKey, int>(_keyComparer);
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
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="EventCounter{TObjectID, TEventID}"/> is reclaimed by garbage collection.
        /// </summary>
        ~EventCounter()
        {
            Debug.Assert(!IsDisposed, "How was this disposed but the destructor called?");

            Dispose(false);
        }

        /// <summary>
        /// Flushes all of the items in the cache.
        /// </summary>
        /// <param name="values">The collection containing the values to flush.</param>
        void Flush(IEnumerable<KeyValuePair<CacheKey, int>> values)
        {
            foreach (var v in values)
            {
                // Only execute the query when the value is non-zero (it is possible to get a zero value when you let your
                // counters decrement)
                if (v.Value != 0)
                {
                    var value = new ObjectEventAmount<TObjectID, TEventID>(v.Key.ObjectID, v.Key.EventID, v.Value);
                    WriteValue(value);
                }
            }
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
            try
            {
                _query.Execute(value);
            }
            catch (Exception ex)
            {
                const string errmsg =
                    "Failed to execute EventCounter query on `{0}` (object: {1}, event: {2})." +
                    " Going to drop the value (value: {3}) instead of retrying. There is a good chance that the exception was caused from" +
                    " trying to insert a record for a foreign key that no longer exists, and a much smaller chance that we tried" +
                    " to insert a record for a foreign key that does not yet exist. Exception: {4}";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this, value.ObjectID, value.EventID, value.Amount, ex);
            }
        }

        #region IEventCounter<TObjectID,TEventID> Members

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
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
                var cache = CreateUpdateCache();
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
            // Do nothing when the amount is zero (no point in incrementing by zero)
            if (amount == 0)
                return;

            // Create the key
            var key = new CacheKey(source, e);

            var flushValue = 0;

            lock (_updateCacheSync)
            {
                var mustAddValue = false;

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
                    const string errmsg =
                        "Overflow occured for source `{0}`, event `{1}`. Flushing buffer to resolve issue," +
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

        #endregion

        /// <summary>
        /// The struct used as the key in the cache.
        /// </summary>
        struct CacheKey : IEquatable<CacheKey>
        {
            /// <summary>
            /// The object ID.
            /// </summary>
            public readonly TObjectID ObjectID;

            /// <summary>
            /// The event ID.
            /// </summary>
            public readonly TEventID EventID;

            /// <summary>
            /// Initializes a new instance of the <see cref="EventCounter{TObjectID, TEventID}.CacheKey"/> struct.
            /// </summary>
            /// <param name="objectID">The object ID.</param>
            /// <param name="eventID">The event ID.</param>
            public CacheKey(TObjectID objectID, TEventID eventID)
            {
                ObjectID = objectID;
                EventID = eventID;
            }

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            /// <returns>
            /// A 32-bit signed integer that is the hash code for this instance.
            /// </returns>
            public override int GetHashCode()
            {
                return _keyComparer.GetHashCode(this);
            }

            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            /// <param name="obj">Another object to compare to.</param>
            /// <returns>
            /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                Debug.Fail("This shouldn't be getting called since CacheKeyComparer should be handling Equals directly.");

                if (obj is CacheKey)
                    return Equals((CacheKey)obj);
                else
                    return false;
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(CacheKey other)
            {
                Debug.Fail("This shouldn't be getting called since CacheKeyComparer should be handling Equals directly.");

                return _keyComparer.Equals(this, other);
            }
        }

        /// <summary>
        /// The <see cref="IEqualityComparer{T}"/> to use for comparing the <see cref="CacheKey"/>.
        /// Provides significant speed improvements over the default comparer by using dynamically generated
        /// methods to allow for equality comparison without boxing structs.
        /// </summary>
        class CacheKeyComparer : IEqualityComparer<CacheKey>
        {
            static readonly Func<CacheKey, CacheKey, bool> _equalityComparerFunc = CreateEqualityComparer();

            /// <summary>
            /// Generates the func to compare the values in the cache key.
            /// </summary>
            /// <returns>The func to compare the values in the cache key.</returns>
            static Func<CacheKey, CacheKey, bool> CreateEqualityComparer()
            {
                /*
                 * The generated func behaves like a method with the following code:
                 * 
                 *  bool Equals(CacheKey x, CacheKey y)
                 *  {
                 *      return x.ObjectID == y.ObjectID && x.EventID == y.EventID;
                 *  }
                 *  
                 */

                var xKey = Expression.Parameter(typeof(CacheKey), "x");
                var yKey = Expression.Parameter(typeof(CacheKey), "y");

                var xKeyA = Expression.PropertyOrField(xKey, "ObjectID");
                var yKeyA = Expression.PropertyOrField(yKey, "ObjectID");
                var eqA = Expression.Equal(xKeyA, yKeyA);

                var xKeyB = Expression.PropertyOrField(xKey, "EventID");
                var yKeyB = Expression.PropertyOrField(yKey, "EventID");
                var eqB = Expression.Equal(xKeyB, yKeyB);

                var andExp = Expression.AndAlso(eqA, eqB);

                var body = Expression.Lambda<Func<CacheKey, CacheKey, bool>>(andExp, new[] { xKey, yKey });

                return body.Compile();
            }

            #region IEqualityComparer<EventCounter<TObjectID,TEventID>.CacheKey> Members

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(CacheKey x, CacheKey y)
            {
                return _equalityComparerFunc(x, y);
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified object.</returns>
            /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
            public int GetHashCode(CacheKey obj)
            {
                return obj.EventID.GetHashCode() ^ obj.ObjectID.GetHashCode();
            }

            #endregion
        }
    }
}