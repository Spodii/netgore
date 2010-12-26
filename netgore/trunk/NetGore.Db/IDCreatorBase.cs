using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NetGore.Db
{
    /// <summary>
    /// A thread-safe object that is used to get and track free IDs. It makes use of the database
    /// to find the free IDs when needed.
    /// </summary>
    /// <typeparam name="T">The Type of the ID.</typeparam>
    public abstract class IDCreatorBase<T> : IDisposable
    {
        readonly Stack<int> _freeIndices;
        readonly SelectIDQuery _selectIDQuery;
        readonly object _stackLock;
        bool _isRefilling;

        /// <summary>
        /// Initializes a new instance of the <see cref="IDCreatorBase{T}"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use to communicate with the database.</param>
        /// <param name="table">Table containing the column to track the values in.</param>
        /// <param name="column">Column containing the IDs to track.</param>
        /// <param name="stackSize">Maximum size of the free ID stack.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="table" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="column" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>stackSize</c> is out of range.</exception>
        protected IDCreatorBase(DbConnectionPool connectionPool, string table, string column, int stackSize)
        {
            if (connectionPool == null)
                throw new ArgumentNullException("connectionPool");
            if (stackSize < 1)
                throw new ArgumentOutOfRangeException("stackSize", "stackSize must be >= 1.");
            if (string.IsNullOrEmpty(table))
                throw new ArgumentNullException("table");
            if (string.IsNullOrEmpty(column))
                throw new ArgumentNullException("column");

            _stackLock = new object();
            _freeIndices = new Stack<int>(stackSize);
            _selectIDQuery = new SelectIDQuery(connectionPool, table, column);

            // Perform the initial fill
            BeginRefill();
        }

        void BeginRefill()
        {
            lock (_stackLock)
            {
                // Make sure we are not already refilling
                if (_isRefilling)
                    return;

                _isRefilling = true;

                // Start the refill thread
                var refillThread = new Thread(Refill) { Name = "ID Refiller" };
                refillThread.Start();
            }
        }

        /// <summary>
        /// Returns an ID to the collection to be reused. Only call this if you are positive the ID is free.
        /// Not every ID has to be freed, and it is assumed at least some will be lost.
        /// </summary>
        /// <param name="id">ID value to freed.</param>
        public virtual void FreeID(T id)
        {
            var value = ToInt(id);

            lock (_stackLock)
            {
                _freeIndices.Push(value);
            }
        }

        /// <summary>
        /// When overridden in the derived class, converts the given int to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The value to convert to type <typeparamref name="T"/>.</param>
        /// <returns>The <paramref name="value"/> as type <typeparamref name="T"/>.</returns>
        protected abstract T FromInt(int value);

        /// <summary>
        /// Finds free indices for a given field from the database.
        /// </summary>
        /// <param name="amount">Total number of values to find from the database.</param>
        /// <returns>A collection with a total of <paramref name="amount"/> Int32s, where each value is
        /// a free index on the column in table. This
        /// collection is sorted ascending by nature.</returns>
        List<int> GetFreeFromDB(int amount)
        {
            // This is just used as a wrapper to easily check and validate the values returned
            // when the DEBUG flag is defined
            var ret = InternalGetFreeFromDB(amount);

            Debug.Assert(ret.Count == amount, "The return count should always equal amount exactly.");

            return ret;
        }

        /// <summary>
        /// Gets the next free ID.
        /// </summary>
        /// <returns>The next free ID.</returns>
        public virtual T GetNext()
        {
            // Just keep looping until we return something
            while (true)
            {
                lock (_stackLock)
                {
                    // Return only if we have something available
                    if (_freeIndices.Count > 0)
                    {
                        var value = _freeIndices.Pop();
                        return FromInt(value);
                    }
                }

                // Nothing was available, so we ensure we're in the process of refilling and keep trying
                BeginRefill();
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Finds free indices for a given field from the database.
        /// </summary>
        /// <param name="amount">Total number of values to find from the database.</param>
        /// <returns>A collection with a total of <paramref name="amount"/> Int32s, where each value is
        /// a free index on the column in table. This
        /// collection is sorted ascending by nature.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>amount</c> is out of range.</exception>
        List<int> InternalGetFreeFromDB(int amount)
        {
            if (amount < 1)
                throw new ArgumentOutOfRangeException("amount", "The amount must be greater than or equal to 1.");

            var returnValues = new List<int>(amount);
            var lastValue = -1;

            // Execute the reader
            using (var r = _selectIDQuery.Execute())
            {
                // Read until we run out of rows
                while (r.Read())
                {
                    // Get the value of the index from the row
                    // We know this will be the 0th element since we only SELECT one thing
                    var value = r.GetInt32(0);

                    // If the value is greater than the last value used + 1, this means that there
                    // was a gap of 2 or more numbers between the last value and the current value.
                    // Since the records are sorted, we know that this means all the values in that
                    // gap are free, so add each one to our return collection.
                    if (value > lastValue + 1)
                    {
                        // Add all the values in the gap to the return collection. We must also make
                        // sure we don't add more than we need, so if the gap contains more values than
                        // we need, only grab the amount we need. This is just an alternative to adding
                        // a "returnValues.Count < amount" check on each loop iteration.
                        var needed = amount - returnValues.Count;
                        var gapSize = value - (lastValue + 1);
                        int loopEnd;

                        if (needed > gapSize)
                            loopEnd = value;
                        else
                            loopEnd = lastValue + 1 + needed;

                        for (var i = lastValue + 1; i < loopEnd; i++)
                        {
                            returnValues.Add(i);
                        }

                        // Because we know that we will only add to the return collection at this point
                        // in this loop, we can safely check only here if we have hit the desired
                        // number of return values. If we have, return.
                        if (returnValues.Count >= amount)
                            return returnValues;
                    }

                    // Update the last value to the current value
                    lastValue = value;
                }
            }

            // If we made it this far, that means we read through every record from the database. At this
            // point, we can conclude the highest index used is equal to lastValue, so we are safe to just
            // keep adding every value greater than lastValue until we have enough values.
            var valuesRemaining = amount - returnValues.Count;
            for (var i = 0; i < valuesRemaining; i++)
            {
                returnValues.Add(++lastValue);
            }

            return returnValues;
        }

        /// <summary>
        /// A blocking, thread-safe method that will refill the free ID stack.
        /// </summary>
        void Refill()
        {
            // Get the free values from the database
            var amount = _freeIndices.Count;
            if (amount < 1)
                amount = 1;

            var freeValues = GetFreeFromDB(amount);

            // Reverse the values so we end up using the lowest values first
            freeValues.Reverse();

            // Lock the stack and add all the new values
            lock (_stackLock)
            {
                foreach (var value in freeValues)
                {
                    _freeIndices.Push(value);
                }
            }

            // Done refilling
            _isRefilling = false;
        }

        /// <summary>
        /// When overridden in the derived class, converts the given value of type <typeparamref name="T"/> to
        /// an int.
        /// </summary>
        /// <param name="value">The value to convert to an int.</param>
        /// <returns>The int value of the <paramref name="value"/>.</returns>
        protected abstract int ToInt(T value);

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_selectIDQuery != null)
                _selectIDQuery.Dispose();
        }

        #endregion

        /// <summary>
        /// The query used to select the IDs from a table.
        /// </summary>
        class SelectIDQuery : DbQueryReader
        {
            const string _queryStr = "SELECT `{0}` FROM `{1}` ORDER BY `{0}` ASC";

            /// <summary>
            /// Initializes a new instance of the <see cref="SelectIDQuery"/> class.
            /// </summary>
            /// <param name="connectionPool">The connection pool.</param>
            /// <param name="table">The table.</param>
            /// <param name="column">The column.</param>
            public SelectIDQuery(DbConnectionPool connectionPool, string table, string column)
                : base(connectionPool, string.Format(_queryStr, column, table))
            {
            }

            /// <summary>
            /// Executes the query.
            /// </summary>
            /// <returns>The <see cref="IDataReader"/> to use to read the query.</returns>
            public IDataReader Execute()
            {
                return ExecuteReader();
            }
        }
    }
}