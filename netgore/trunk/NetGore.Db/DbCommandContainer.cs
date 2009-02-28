using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Db
{
    /*
    /// <summary>
    /// Container for an IDbCommand that alters the behavior of Dispose to just close the connection used with
    /// the IDbCommand instead of actually disposing of the command.
    /// </summary>
    public sealed class DbCommandContainer : IDbCommand
    {
        readonly IDbCommand _command;
        readonly IDbConnection _connection;

        internal DbCommandContainer(IDbConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            _connection = connection;
            _command = connection.CreateCommand();
        }

        /// <summary>
        /// Performs the actual disposal of the internal IDbCommand in this DbCommandContainer.
        /// </summary>
        internal void InternalDispose()
        {
            // NOTE: Unused
            _command.Dispose();
        }

        /// <summary>
        /// Activates the DbCommandContainer, getting it ready for being used.
        /// </summary>
        internal void Activate()
        {
            _connection.Open();
        }

        #region IDbCommandContainer Members

        ///<summary>
        ///
        ///                    Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///                
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            _command.CommandType = CommandType.Text;
            _command.CommandText = string.Empty;
            _command.Parameters.Clear();
            _connection.Close();
        }

        #endregion

        ///<summary>
        ///
        ///                    Creates a prepared (or compiled) version of the command on the data source.
        ///                
        ///</summary>
        ///
        ///<exception cref="T:System.InvalidOperationException">
        ///                    The <see cref="P:System.Data.OleDb.OleDbCommand.Connection" /> is not set.
        ///                
        ///                    -or- 
        ///                
        ///                    The <see cref="P:System.Data.OleDb.OleDbCommand.Connection" /> is not <see cref="M:System.Data.OleDb.OleDbConnection.Open" />. 
        ///                </exception><filterpriority>2</filterpriority>
        public void Prepare()
        {
            _command.Prepare();
        }

        ///<summary>
        ///
        ///                    Attempts to cancels the execution of an <see cref="T:System.Data.IDbCommand" />.
        ///                
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Cancel()
        {
            _command.Cancel();
        }

        ///<summary>
        ///
        ///                    Creates a new instance of an <see cref="T:System.Data.IDbDataParameter" /> object.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    An IDbDataParameter object.
        ///                
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public IDbDataParameter CreateParameter()
        {
            return _command.CreateParameter();
        }

        ///<summary>
        ///
        ///                    Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The number of rows affected.
        ///                
        ///</returns>
        ///
        ///<exception cref="T:System.InvalidOperationException">
        ///                    The connection does not exist.
        ///                
        ///                    -or- 
        ///                
        ///                    The connection is not open. 
        ///                </exception><filterpriority>2</filterpriority>
        public int ExecuteNonQuery()
        {
            return _command.ExecuteNonQuery();
        }

        ///<summary>
        ///
        ///                    Executes the <see cref="P:System.Data.IDbCommand.CommandText" /> against the <see cref="P:System.Data.IDbCommand.Connection" /> and builds an <see cref="T:System.Data.IDataReader" />.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    An <see cref="T:System.Data.IDataReader" /> object.
        ///                
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public IDataReader ExecuteReader()
        {
            return _command.ExecuteReader();
        }

        ///<summary>
        ///
        ///                    Executes the <see cref="P:System.Data.IDbCommand.CommandText" /> against the <see cref="P:System.Data.IDbCommand.Connection" />, and builds an <see cref="T:System.Data.IDataReader" /> using one of the <see cref="T:System.Data.CommandBehavior" /> values.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    An <see cref="T:System.Data.IDataReader" /> object.
        ///                
        ///</returns>
        ///
        ///<param name="behavior">
        ///                    One of the <see cref="T:System.Data.CommandBehavior" /> values. 
        ///                </param><filterpriority>2</filterpriority>
        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return _command.ExecuteReader(behavior);
        }

        ///<summary>
        ///
        ///                    Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The first column of the first row in the resultset.
        ///                
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public object ExecuteScalar()
        {
            return _command.ExecuteScalar();
        }

        ///<summary>
        ///
        ///                    Gets or sets the <see cref="T:System.Data.IDbConnection" /> used by this instance of the <see cref="T:System.Data.IDbCommand" />.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The connection to the data source.
        ///                
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public IDbConnection Connection
        {
            get { return _command.Connection; }
            set { _command.Connection = value; }
        }

        ///<summary>
        ///
        ///                    Gets or sets the transaction within which the Command object of a .NET Framework data provider executes.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    the Command object of a .NET Framework data provider executes. The default value is null.
        ///                
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public IDbTransaction Transaction
        {
            get { return _command.Transaction; }
            set { _command.Transaction = value; }
        }

        ///<summary>
        ///
        ///                    Gets or sets the text command to run against the data source.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The text command to execute. The default value is an empty string ("").
        ///                
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public string CommandText
        {
            get { return _command.CommandText; }
            set { _command.CommandText = value; }
        }

        ///<summary>
        ///
        ///                    Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The time (in seconds) to wait for the command to execute. The default value is 30 seconds.
        ///                
        ///</returns>
        ///
        ///<exception cref="T:System.ArgumentException">
        ///                    The property value assigned is less than 0. 
        ///                </exception><filterpriority>2</filterpriority>
        public int CommandTimeout
        {
            get { return _command.CommandTimeout; }
            set { _command.CommandTimeout = value; }
        }

        ///<summary>
        ///
        ///                    Indicates or specifies how the <see cref="P:System.Data.IDbCommand.CommandText" /> property is interpreted.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    One of the <see cref="T:System.Data.CommandType" /> values. The default is Text.
        ///                
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public CommandType CommandType
        {
            get { return _command.CommandType; }
            set { _command.CommandType = value; }
        }

        ///<summary>
        ///
        ///                    Gets the <see cref="T:System.Data.IDataParameterCollection" />.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The parameters of the SQL statement or stored procedure.
        ///                
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public IDataParameterCollection Parameters
        {
            get { return _command.Parameters; }
        }

        ///<summary>
        ///
        ///                    Gets or sets how command results are applied to the <see cref="T:System.Data.DataRow" /> when used by the <see cref="M:System.Data.IDataAdapter.Update(System.Data.DataSet)" /> method of a <see cref="T:System.Data.Common.DbDataAdapter" />.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    One of the <see cref="T:System.Data.UpdateRowSource" /> values. The default is Both unless the command is automatically generated. Then the default is None.
        ///                
        ///</returns>
        ///
        ///<exception cref="T:System.ArgumentException">
        ///                    The value entered was not one of the <see cref="T:System.Data.UpdateRowSource" /> values. 
        ///                </exception><filterpriority>2</filterpriority>
        public UpdateRowSource UpdatedRowSource
        {
            get { return _command.UpdatedRowSource; }
            set { _command.UpdatedRowSource = value; }
        }
    }
    */
}