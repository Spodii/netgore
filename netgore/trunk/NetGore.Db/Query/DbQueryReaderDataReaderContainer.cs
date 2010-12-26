using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;

namespace NetGore.Db
{
    /// <summary>
    /// Container for the <see cref="DbQueryReader"/> that will allow us to safely and properly dispose of the reader and poolable
    /// connection by just calling Dispose() on it.
    /// </summary>
    sealed class DbQueryReaderDataReaderContainer : DataReaderContainer
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly Stack<DbQueryReaderDataReaderContainer> _pool = new Stack<DbQueryReaderDataReaderContainer>();
        static readonly object _poolSync = new object();

        DbCommand _command;
        DbQueryBase _dbQueryBase;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryReaderDataReaderContainer"/> class.
        /// </summary>
        DbQueryReaderDataReaderContainer()
        {
        }

        /// <summary>
        /// Creates a <see cref="DbQueryReaderDataReaderContainer"/>.
        /// </summary>
        /// <param name="dbQueryBase">The <see cref="DbQueryBase"/>.</param>
        /// <param name="command">The <see cref="DbCommand"/>.</param>
        /// <param name="dataReader">The <see cref="IDataReader"/>.</param>
        /// <returns>The <see cref="DbQueryReaderDataReaderContainer"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dbQueryBase" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="command" /> is <c>null</c>.</exception>
        public static DbQueryReaderDataReaderContainer Create(DbQueryBase dbQueryBase, DbCommand command, IDataReader dataReader)
        {
            if (dbQueryBase == null)
                throw new ArgumentNullException("dbQueryBase");
            if (command == null)
                throw new ArgumentNullException("command");

            DbQueryReaderDataReaderContainer r = null;

            // Try to grab from the pool first
            lock (_poolSync)
            {
                if (_pool.Count > 0)
                    r = _pool.Pop();
            }

            // Couldn't grab from pool - create new instance
            if (r == null)
                r = new DbQueryReaderDataReaderContainer();

            Debug.Assert(r._dbQueryBase == null);
            Debug.Assert(r._command == null);
            Debug.Assert(r.DataReader == null);

            // Initialize
            r._dbQueryBase = dbQueryBase;
            r._command = command;
            r.DataReader = dataReader;

            return r;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        [SuppressMessage("Microsoft.Usage", "CA2215:Dispose methods should call base class dispose")]
        protected override void Dispose(bool disposeManaged)
        {
            try
            {
                if (DataReader != null)
                    DataReader.Dispose();
            }
            finally
            {
                try
                {
                    _dbQueryBase.ConnectionPool.QueryRunner.EndExecuteReader();
                }
                catch (SynchronizationLockException ex)
                {
                    const string errmsg = "Failed to release reader on `{0}` for query `{1}`. Exception: {2}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this, _command, ex);
                    Debug.Fail(string.Format(errmsg, this, _command, ex));
                }
                finally
                {
                    try
                    {
                        _dbQueryBase.ReleaseCommand(_command);
                    }
                    finally
                    {
                        // Release the object references
                        _command = null;
                        _dbQueryBase = null;
                        DataReader = null;

                        // Throw back into the pool
                        lock (_poolSync)
                        {
                            Debug.Assert(!_pool.Contains(this));
                            _pool.Push(this);
                        }
                    }
                }
            }
        }
    }
}