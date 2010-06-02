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
    /// Class that wraps around a DbParameterCollection, exposing only the DbParameter's value. 
    /// </summary>
    public sealed class DbParameterValues : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// DbParameterCollection that this DbParameterValues exposes the values of.
        /// </summary>
        readonly DbParameterCollection _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbParameterValues"/> class.
        /// </summary>
        /// <param name="dbParameterCollection">DbParameterCollection to wrap around.</param>
        public DbParameterValues(DbParameterCollection dbParameterCollection)
        {
            if (dbParameterCollection == null)
                throw new ArgumentNullException("dbParameterCollection");

            _collection = dbParameterCollection;
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

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
                const string errmsg = "Parameter `{0}` explicitly specified the parameter prefix ({1}). It is recommended to not explicitly include the prefix.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, parameterName, DbQueryBase.ParameterPrefix);
                Debug.Fail(string.Format(errmsg, parameterName, DbQueryBase.ParameterPrefix));
            }

            return parameterName;
        }

        /// <summary>
        /// Gets the number of parameters in this collection.
        /// </summary>
        public int Count
        {
            get { return _collection.Count; }
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
        /// Gets the name of the parameter at the given index.
        /// </summary>
        /// <param name="index">Index of the parameter to get the name of.</param>
        /// <returns>The name of the parameter at the given index.</returns>
        public string GetParameterName(int index)
        {
            return _collection[index].ParameterName;
        }

        #region IEnumerable<KeyValuePair<string,object>> Members

        ///<summary>
        ///
        ///                    Returns an enumerator that iterates through the collection.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        ///                
        ///</returns>
        ///
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

        ///<summary>
        ///
        ///                    Returns an enumerator that iterates through a collection.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        ///                
        ///</returns>
        ///
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}