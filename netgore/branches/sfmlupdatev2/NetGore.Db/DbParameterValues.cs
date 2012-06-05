using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Db
{
    /// <summary>
    /// Class that wraps around a <see cref="DbParameterCollection"/>, exposing only the <see cref="DbParameter"/>'s value
    /// and providing a more consistent and safe way of referring to items in the collection.
    /// </summary>
    public sealed class DbParameterValues : IEnumerable<KeyValuePair<string, object>>, IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly Stack<DbParameterValues> _pool = new Stack<DbParameterValues>();
        static readonly object _poolLock = new object();

        /// <summary>
        /// The <see cref="DbParameterCollection"/> that this <see cref="DbParameterValues"/> exposes the values of.
        /// </summary>
        DbParameterCollection _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbParameterValues"/> class.
        /// </summary>
        DbParameterValues()
        {
        }

        /// <summary>
        /// Gets or sets the parameter's value.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter.</param>
        /// <returns>Value of the parameter at the given <paramref name="index"/>.</returns>
        public object this[int index]
        {
            get { return _collection[index].Value; }
            set { _collection[index].Value = value; }
        }

        /// <summary>
        /// Gets or sets the parameter's value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>Value of the parameter with the given <paramref name="parameterName"/>.</returns>
        public object this[string parameterName]
        {
            get
            {
                parameterName = AddParameterPrefixIfNeeded(parameterName);
                return _collection[parameterName].Value;
            }
            set
            {
                parameterName = AddParameterPrefixIfNeeded(parameterName);
                _collection[parameterName].Value = value;
            }
        }

        /// <summary>
        /// Gets the number of parameters in this collection.
        /// </summary>
        public int Count
        {
            get { return _collection.Count; }
        }

        /// <summary>
        /// Adds the <see cref="DbQueryBase.ParameterPrefix"/> to the <paramref name="parameterName"/> if needed.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to add the prefix to.</param>
        /// <returns>The <paramref name="parameterName"/> with the <see cref="DbQueryBase.ParameterPrefix"/>.</returns>
        static string AddParameterPrefixIfNeeded(string parameterName)
        {
            if (!parameterName.StartsWith(DbQueryBase.ParameterPrefix))
            {
                // Add the parameter prefix
                parameterName = DbQueryBase.ParameterPrefix + parameterName;
            }
            else
            {
                // Parameter prefix was already on the string
                const string errmsg =
                    "Parameter `{0}` explicitly specified the parameter prefix ({1}). It is recommended to not explicitly include the prefix.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, parameterName, DbQueryBase.ParameterPrefix);
                Debug.Fail(string.Format(errmsg, parameterName, DbQueryBase.ParameterPrefix));
            }

            return parameterName;
        }

        /// <summary>
        /// Checks if the DbParameter with the specified <paramref name="parameterName"/> exists in this collection.
        /// </summary>
        /// <param name="parameterName">Name of the parameter to check if exists.</param>
        /// <returns>True if the <paramref name="parameterName"/> exists in this collection.</returns>
        public bool Contains(string parameterName)
        {
            parameterName = AddParameterPrefixIfNeeded(parameterName);
            return _collection.Contains(parameterName);
        }

        /// <summary>
        /// Creates a <see cref="DbParameterValues"/> for a <see cref="DbParameterCollection"/>.
        /// </summary>
        /// <param name="dbParameterCollection">The <see cref="DbParameterCollection"/>.</param>
        /// <returns>The <see cref="DbParameterValues"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dbParameterCollection" /> is <c>null</c>.</exception>
        public static DbParameterValues Create(DbParameterCollection dbParameterCollection)
        {
            if (dbParameterCollection == null)
                throw new ArgumentNullException("dbParameterCollection");

            // Grab from the pool first
            DbParameterValues ret = null;
            lock (_poolLock)
            {
                if (_pool.Count > 0)
                    ret = _pool.Pop();
            }

            // Only create a new instance if we have to (nothing came from the pool)
            if (ret == null)
                ret = new DbParameterValues();

            Debug.Assert(ret._collection == null, "Since Dispose() sets the _collection to null, how did this happen?");

            // Set the internal collection
            ret._collection = dbParameterCollection;

            return ret;
        }

        /// <summary>
        /// Gets the name of the parameter at the given index.
        /// </summary>
        /// <param name="index">Index of the parameter to get the name of.</param>
        /// <returns>The name of the parameter at the given index.</returns>
        public string GetParameterName(int index)
        {
            var ret = _collection[index].ParameterName;
            if (ret.StartsWith(DbQueryBase.ParameterPrefix))
                ret = ret.Substring(1);

            Debug.Assert(!ret.StartsWith(DbQueryBase.ParameterPrefix));
            Debug.Assert(ret.Length > 0);

            return ret;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_collection == null)
            {
                Debug.Fail("Object already disposed!");
                return;
            }

            _collection = null;

            lock (_poolLock)
                _pool.Push(this);
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,object>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (DbParameter parameter in _collection)
            {
                Debug.Assert(parameter.ParameterName.StartsWith(DbQueryBase.ParameterPrefix));

                var nameWithoutPrefix = parameter.ParameterName.Substring(1);

                Debug.Assert(!nameWithoutPrefix.StartsWith(DbQueryBase.ParameterPrefix));

                yield return new KeyValuePair<string, object>(nameWithoutPrefix, parameter.Value);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}