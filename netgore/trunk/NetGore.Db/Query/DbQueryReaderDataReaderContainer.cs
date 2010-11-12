using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;

namespace NetGore.Db
{
    /// <summary>
    /// Container for the DbQueryReader that will allow us to safely and properly dispose of the reader and poolable
    /// connection by just calling Dispose() on it.
    /// </summary>
    sealed class DbQueryReaderDataReaderContainer : DataReaderContainer
    {
        // TODO: !! Pool
        readonly DbQueryBase _dbQueryBase;
        readonly DbCommand _command;

        public DbQueryReaderDataReaderContainer(DbQueryBase dbQueryBase, DbCommand command, IDataReader dataReader) : base(dataReader)
        {
            if (dbQueryBase == null)
                throw new ArgumentNullException("dbQueryBase");
            if (command == null)
                throw new ArgumentNullException("command");

            _dbQueryBase = dbQueryBase;
            _command = command;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposeManaged)
        {
            try
            {
                if (disposeManaged)
                {
                    if (DataReader != null)
                        DataReader.Dispose();

                    _dbQueryBase.ReleaseCommand(_command);
                }

                base.Dispose(disposeManaged);
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
            }
        }

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}