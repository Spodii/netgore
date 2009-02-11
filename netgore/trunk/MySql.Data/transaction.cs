using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace MySql.Data.MySqlClient
{
    /// <include file='docs/MySqlTransaction.xml' path='docs/Class/*'/>
    public sealed class MySqlTransaction : DbTransaction
    {
        readonly MySqlConnection conn;
        readonly IsolationLevel level;
        bool open;

        /// <summary>
        /// Gets the <see cref="MySqlConnection"/> object associated with the transaction, or a null reference (Nothing in Visual Basic) if the transaction is no longer valid.
        /// </summary>
        /// <value>The <see cref="MySqlConnection"/> object associated with this transaction.</value>
        /// <remarks>
        /// A single application may have multiple database connections, each 
        /// with zero or more transactions. This property enables you to 
        /// determine the connection object associated with a particular 
        /// transaction created by <see cref="MySqlConnection.BeginTransaction()"/>.
        /// </remarks>
        public new MySqlConnection Connection
        {
            get { return conn; }
        }

        ///<summary>
        ///
        ///                    Specifies the <see cref="T:System.Data.Common.DbConnection" /> object associated with the transaction.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The <see cref="T:System.Data.Common.DbConnection" /> object associated with the transaction.
        ///                
        ///</returns>
        ///
        protected override DbConnection DbConnection
        {
            get { return conn; }
        }

        /// <summary>
        /// Specifies the <see cref="IsolationLevel"/> for this transaction.
        /// </summary>
        /// <value>
        /// The <see cref="IsolationLevel"/> for this transaction. The default is <b>ReadCommitted</b>.
        /// </value>
        /// <remarks>
        /// Parallel transactions are not supported. Therefore, the IsolationLevel 
        /// applies to the entire transaction.
        /// </remarks>
        public override IsolationLevel IsolationLevel
        {
            get { return level; }
        }

        internal MySqlTransaction(MySqlConnection c, IsolationLevel il)
        {
            conn = c;
            level = il;
            open = true;
        }

        /// <include file='docs/MySqlTransaction.xml' path='docs/Commit/*'/>
        public override void Commit()
        {
            if (conn == null || (conn.State != ConnectionState.Open && !conn.SoftClosed))
                throw new InvalidOperationException("Connection must be valid and open to commit transaction");
            if (!open)
                throw new InvalidOperationException("Transaction has already been committed or is not pending");
            MySqlCommand cmd = new MySqlCommand("COMMIT", conn);
            cmd.ExecuteNonQuery();
            open = false;
        }

        ///<summary>
        ///
        ///                    Releases the unmanaged resources used by the <see cref="T:System.Data.Common.DbTransaction" /> and optionally releases the managed resources.
        ///                
        ///</summary>
        ///
        ///<param name="disposing">
        ///                    If true, this method releases all resources held by any managed objects that this <see cref="T:System.Data.Common.DbTransaction" /> references.
        ///                </param>
        protected override void Dispose(bool disposing)
        {
            if ((conn != null && conn.State == ConnectionState.Open || conn.SoftClosed) && open)
                Rollback();
            base.Dispose(disposing);
        }

        /// <include file='docs/MySqlTransaction.xml' path='docs/Rollback/*'/>
        public override void Rollback()
        {
            if (conn == null || (conn.State != ConnectionState.Open && !conn.SoftClosed))
                throw new InvalidOperationException("Connection must be valid and open to rollback transaction");
            if (!open)
                throw new InvalidOperationException("Transaction has already been rolled back or is not pending");
            MySqlCommand cmd = new MySqlCommand("ROLLBACK", conn);
            cmd.ExecuteNonQuery();
            open = false;
        }
    }
}