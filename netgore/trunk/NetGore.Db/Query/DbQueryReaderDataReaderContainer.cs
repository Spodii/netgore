using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Container for the DbQueryReader that will allow us to safely and properly dispose of the reader and poolable
    /// connection by just calling Dispose() on it.
    /// </summary>
    internal sealed class DbQueryReaderDataReaderContainer : DataReaderContainer
    {
        readonly DbQueryBase _dbQueryBase;
        readonly IPoolableDbConnection _poolableConn;

        internal DbQueryReaderDataReaderContainer(DbQueryBase dbQueryBase, IPoolableDbConnection poolableConn, IDbCommand command,
                                                  IDataReader dataReader) : base(command, dataReader)
        {
            // We must use a DbCommand on the parameter, not IDbCommand, because Dispose explicitly casts to a DbCommand

            if (dbQueryBase == null)
                throw new ArgumentNullException("dbQueryBase");
            if (poolableConn == null)
                throw new ArgumentNullException("poolableConn");

            _dbQueryBase = dbQueryBase;
            _poolableConn = poolableConn;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposeManaged)
        {
            if (!disposeManaged)
                return;

            DataReader.Dispose();
            _dbQueryBase.ReleaseCommand((DbCommand)Command);
            _poolableConn.Dispose();
        }
    }
}