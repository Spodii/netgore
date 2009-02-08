using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;

namespace MySql.Data.MySqlClient
{
    /// <include file='docs/MySqlDataAdapter.xml' path='docs/class/*'/>
    [ToolboxBitmap(typeof(MySqlDataAdapter), "MySqlClient.resources.dataadapter.bmp")]
    [DesignerCategory("Code")]
    [Designer("MySql.Data.MySqlClient.Design.MySqlDataAdapterDesigner,MySqlClient.Design")]
    public sealed class MySqlDataAdapter : DbDataAdapter, IDataAdapter
    {
        List<IDbCommand> commandBatch;
        bool loadingDefaults;
        int updateBatchSize;

        /// <summary>
        /// Occurs during Update after a command is executed against the data source. The attempt to update is made, so the event fires.
        /// </summary>
        public event MySqlRowUpdatedEventHandler RowUpdated;

        /// <summary>
        /// Occurs during Update before a command is executed against the data source. The attempt to update is made, so the event fires.
        /// </summary>
        public event MySqlRowUpdatingEventHandler RowUpdating;

        /// <include file='docs/MySqlDataAdapter.xml' path='docs/DeleteCommand/*'/>
        [Description("Used during Update for deleted rows in Dataset.")]
        public new MySqlCommand DeleteCommand
        {
            get { return (MySqlCommand)base.DeleteCommand; }
            set { base.DeleteCommand = value; }
        }

        /// <include file='docs/MySqlDataAdapter.xml' path='docs/InsertCommand/*'/>
        [Description("Used during Update for new rows in Dataset.")]
        public new MySqlCommand InsertCommand
        {
            get { return (MySqlCommand)base.InsertCommand; }
            set { base.InsertCommand = value; }
        }

        internal bool LoadDefaults
        {
            get { return loadingDefaults; }
            set { loadingDefaults = value; }
        }

        /// <include file='docs/MySqlDataAdapter.xml' path='docs/SelectCommand/*'/>
        [Description("Used during Fill/FillSchema")]
        [Category("Fill")]
        public new MySqlCommand SelectCommand
        {
            get { return (MySqlCommand)base.SelectCommand; }
            set { base.SelectCommand = value; }
        }

        ///<summary>
        ///
        ///                    Gets or sets a value that enables or disables batch processing support, and specifies the number of commands that can be executed in a batch. 
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The number of rows to process per batch. 
        ///                
        ///                    Value is
        ///                
        ///                    Effect
        ///                
        ///                    0
        ///                
        ///                    There is no limit on the batch size.
        ///                
        ///                    1
        ///                
        ///                    Disables batch updating.
        ///                
        ///                    &gt; 1
        ///                
        ///                    Changes are sent using batches of <see cref="P:System.Data.Common.DbDataAdapter.UpdateBatchSize" /> operations at a time.
        ///                
        ///                    When setting this to a value other than 1 ,all the commands associated with the <see cref="T:System.Data.Common.DbDataAdapter" /> must have their <see cref="P:System.Data.IDbCommand.UpdatedRowSource" /> property set to None or OutputParameters. An exception will be thrown otherwise. 
        ///                
        ///</returns>
        ///<filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" /></PermissionSet>
        public override int UpdateBatchSize
        {
            get { return updateBatchSize; }
            set { updateBatchSize = value; }
        }

        /// <include file='docs/MySqlDataAdapter.xml' path='docs/UpdateCommand/*'/>
        [Description("Used during Update for modified rows in Dataset.")]
        public new MySqlCommand UpdateCommand
        {
            get { return (MySqlCommand)base.UpdateCommand; }
            set { base.UpdateCommand = value; }
        }

        /// <include file='docs/MySqlDataAdapter.xml' path='docs/Ctor/*'/>
        public MySqlDataAdapter()
        {
            loadingDefaults = true;
            updateBatchSize = 1;
        }

        /// <include file='docs/MySqlDataAdapter.xml' path='docs/Ctor1/*'/>
        public MySqlDataAdapter(MySqlCommand selectCommand) : this()
        {
            SelectCommand = selectCommand;
        }

        /// <include file='docs/MySqlDataAdapter.xml' path='docs/Ctor2/*'/>
        public MySqlDataAdapter(string selectCommandText, MySqlConnection connection) : this()
        {
            SelectCommand = new MySqlCommand(selectCommandText, connection);
        }

        /// <include file='docs/MySqlDataAdapter.xml' path='docs/Ctor3/*'/>
        public MySqlDataAdapter(string selectCommandText, string selectConnString) : this()
        {
            SelectCommand = new MySqlCommand(selectCommandText, new MySqlConnection(selectConnString));
        }

        ///<summary>
        ///
        ///                    Adds a <see cref="T:System.Data.IDbCommand" /> to the current batch.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The number of commands in the batch before adding the <see cref="T:System.Data.IDbCommand" />.
        ///                
        ///</returns>
        ///
        ///<param name="command">
        ///                    The <see cref="T:System.Data.IDbCommand" /> to add to the batch.
        ///                </param>
        ///<exception cref="T:System.NotSupportedException">
        ///                    The adapter does not support batches. 
        ///                </exception>
        protected override int AddToBatch(IDbCommand command)
        {
            // the first time each command is asked to be batched, we ask
            // that command to prepare its batchable command text.  We only want
            // to do this one time for each command
            MySqlCommand commandToBatch = (MySqlCommand)command;
            if (commandToBatch.BatchableCommandText == null)
                commandToBatch.GetCommandTextForBatching();

            IDbCommand cloneCommand = (IDbCommand)((ICloneable)command).Clone();
            commandBatch.Add(cloneCommand);

            return commandBatch.Count - 1;
        }

        ///<summary>
        ///
        ///                    Removes all <see cref="T:System.Data.IDbCommand" /> objects from the batch.
        ///                
        ///</summary>
        ///
        ///<exception cref="T:System.NotSupportedException">
        ///                    The adapter does not support batches. 
        ///                </exception>
        protected override void ClearBatch()
        {
            if (commandBatch.Count > 0)
            {
                MySqlCommand cmd = (MySqlCommand)commandBatch[0];
                if (cmd.Batch != null)
                    cmd.Batch.Clear();
            }
            commandBatch.Clear();
        }

        /*		protected override int Fill(DataTable dataTable, IDataReader dataReader)
		{
			int result = base.Fill (dataTable, dataReader);
			//LoadDefaultValues(dataTable, dataReader);
			return result;
		}

		protected override int Fill(DataSet dataSet, string srcTable, IDataReader dataReader, int startRecord, int maxRecords)
		{
			int result = base.Fill (dataSet, srcTable, dataReader, startRecord, maxRecords);
			//LoadDefaultValues(dataSet.Tables[srcTable], dataReader);
			return result;
		}

*/
/*		private void LoadDefaultValues(DataTable dataTable, IDataReader reader)
		{
			if (! loadingDefaults) return;
			if (dataTable.ExtendedProperties["DefaultsChecked"] != null) return;
			if (this.MissingSchemaAction != MissingSchemaAction.Add &&
				this.MissingSchemaAction != MissingSchemaAction.AddWithKey) return;
			
			DataTable schemaTable = reader.GetSchemaTable();
			reader.Close();

			DatabaseMetaData dmd = new DatabaseMetaData(this.SelectCommand.Connection);

			foreach (DataRow row in schemaTable.Rows)
			{
				DataRow dmdRow = dmd.GetColumn(row["BaseCatalogName"].ToString(), 
					null, row["BaseTableName"].ToString(), row["BaseColumnName"].ToString() );
				object defaultVal = dmdRow["COLUMN_DEFAULT"];
				DataColumn col = dataTable.Columns[row["ColumnName"].ToString()];
				if (defaultVal != System.DBNull.Value)
				{
					if (! col.AllowDBNull) col.ExtendedProperties.Add("UseDefault", true);
					col.AllowDBNull = true;
					mayUseDefault = true;
				}
			}

			dataTable.ExtendedProperties.Add("DefaultsChecked", true);
		}

*/

/*		private void FixupStatementDefaults(RowUpdatingEventArgs args)
		{
			DataTable table = args.Row.Table;
			DataRow row = args.Row;

			savedSql = args.Command.CommandText;
			string newSql = savedSql;

			if (mayUseDefault)
				this.InsertCommand.Unprepare();

			foreach (IDataParameter p in args.Command.Parameters)
			{
				if (row[p.SourceColumn] != DBNull.Value) continue;
				DataColumn col = table.Columns[p.SourceColumn];
				if (! col.ExtendedProperties.ContainsKey("UseDefault")) continue;
				newSql = newSql.Replace("?"+p.ParameterName, "DEFAULT");
			}
			args.Command.CommandText = newSql;
		}
*/
        /*
		* Implement abstract methods inherited from DbDataAdapter.
		*/

        /// <summary>
        /// Overridden. See <see cref="DbDataAdapter.CreateRowUpdatedEvent"/>.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="command"></param>
        /// <param name="statementType"></param>
        /// <param name="tableMapping"></param>
        /// <returns></returns>
        protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command,
                                                                     StatementType statementType, DataTableMapping tableMapping)
        {
            return new MySqlRowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
        }

        /// <summary>
        /// Overridden. See <see cref="DbDataAdapter.CreateRowUpdatingEvent"/>.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="command"></param>
        /// <param name="statementType"></param>
        /// <param name="tableMapping"></param>
        /// <returns></returns>
        protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command,
                                                                       StatementType statementType, DataTableMapping tableMapping)
        {
            return new MySqlRowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
        }

        ///<summary>
        ///
        ///                    Executes the current batch.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The return value from the last command in the batch.
        ///                
        ///</returns>
        ///
        protected override int ExecuteBatch()
        {
            int recordsAffected = 0;
            int index = 0;
            while (index < commandBatch.Count)
            {
                MySqlCommand cmd = (MySqlCommand)commandBatch[index++];
                for (int index2 = index; index2 < commandBatch.Count; index2++,index++)
                {
                    MySqlCommand cmd2 = (MySqlCommand)commandBatch[index2];
                    if (cmd2.CommandText != cmd.CommandText)
                        break;
                    cmd.AddToBatch(cmd2);
                }
                recordsAffected += cmd.ExecuteNonQuery();
            }
            return recordsAffected;
        }

        ///<summary>
        ///
        ///                    Returns a <see cref="T:System.Data.IDataParameter" /> from one of the commands in the current batch.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The <see cref="T:System.Data.IDataParameter" /> specified.
        ///                
        ///</returns>
        ///
        ///<param name="commandIdentifier">
        ///                    The index of the command to retrieve the parameter from.
        ///                </param>
        ///<param name="parameterIndex">
        ///                    The index of the parameter within the command.
        ///                </param>
        ///<exception cref="T:System.NotSupportedException">
        ///                    The adapter does not support batches. 
        ///                </exception>
        protected override IDataParameter GetBatchedParameter(int commandIdentifier, int parameterIndex)
        {
            return (IDataParameter)commandBatch[commandIdentifier].Parameters[parameterIndex];
        }

        ///<summary>
        ///
        ///                    Initializes batching for the <see cref="T:System.Data.Common.DbDataAdapter" />.
        ///                
        ///</summary>
        ///
        ///<exception cref="T:System.NotSupportedException">
        ///                    The adapter does not support batches. 
        ///                </exception>
        protected override void InitializeBatching()
        {
            commandBatch = new List<IDbCommand>();
        }

        /// <summary>
        /// Overridden. Raises the RowUpdated event.
        /// </summary>
        /// <param name="value">A MySqlRowUpdatedEventArgs that contains the event data. </param>
        protected override void OnRowUpdated(RowUpdatedEventArgs value)
        {
//			MySqlRowUpdatedEventArgs margs = (value as MySqlRowUpdatedEventArgs);
            //args.Command.CommandText = savedSql;

            if (RowUpdated != null)
                RowUpdated(this, (value as MySqlRowUpdatedEventArgs));

            //          MySqlRowUpdatedEventHandler handler = (MySqlRowUpdatedEventHandler)Events[EventRowUpdated];
            //		if ((null != handler) && (margs != null))
            //		{
            //			handler(this, margs);
            //		}
        }

        /// <summary>
        /// Overridden. Raises the RowUpdating event.
        /// </summary>
        /// <param name="value">A MySqlRowUpdatingEventArgs that contains the event data.</param>
        protected override void OnRowUpdating(RowUpdatingEventArgs value)
        {
            //		MySqlRowUpdatingEventArgs args = (value as MySqlRowUpdatingEventArgs);
            //			if (args.StatementType == StatementType.Insert)
//				FixupStatementDefaults(args);

            if (RowUpdating != null)
                RowUpdating(this, (value as MySqlRowUpdatingEventArgs));

//			MySqlRowUpdatingEventHandler handler = (MySqlRowUpdatingEventHandler) Events[EventRowUpdating];
//			if ((null != handler) && (margs != null)) 
//			{
//				handler(this, margs);
//			}
        }

        ///<summary>
        ///
        ///                    Ends batching for the <see cref="T:System.Data.Common.DbDataAdapter" />.
        ///                
        ///</summary>
        ///
        ///<exception cref="T:System.NotSupportedException">
        ///                    The adapter does not support batches. 
        ///                </exception>
        protected override void TerminateBatching()
        {
            ClearBatch();
            commandBatch = null;
        }
    }

    /// <summary>
    /// Represents the method that will handle the <see cref="MySqlDataAdapter.RowUpdating"/> event of a <see cref="MySqlDataAdapter"/>.
    /// </summary>
    public delegate void MySqlRowUpdatingEventHandler(object sender, MySqlRowUpdatingEventArgs e);

    /// <summary>
    /// Represents the method that will handle the <see cref="MySqlDataAdapter.RowUpdated"/> event of a <see cref="MySqlDataAdapter"/>.
    /// </summary>
    public delegate void MySqlRowUpdatedEventHandler(object sender, MySqlRowUpdatedEventArgs e);

    /// <summary>
    /// Provides data for the RowUpdating event. This class cannot be inherited.
    /// </summary>
    public sealed class MySqlRowUpdatingEventArgs : RowUpdatingEventArgs
    {
        /// <summary>
        /// Gets or sets the MySqlCommand to execute when performing the Update.
        /// </summary>
        public new MySqlCommand Command
        {
            get { return (MySqlCommand)base.Command; }
            set { base.Command = value; }
        }

        /// <summary>
        /// Initializes a new instance of the MySqlRowUpdatingEventArgs class.
        /// </summary>
        /// <param name="row">The <see cref="DataRow"/> to 
        /// <see cref="DbDataAdapter.Update(DataSet)"/>.</param>
        /// <param name="command">The <see cref="IDbCommand"/> to execute during <see cref="DbDataAdapter.Update(DataSet)"/>.</param>
        /// <param name="statementType">One of the <see cref="StatementType"/> values that specifies the type of query executed.</param>
        /// <param name="tableMapping">The <see cref="DataTableMapping"/> sent through an <see cref="DbDataAdapter.Update(DataSet)"/>.</param>
        public MySqlRowUpdatingEventArgs(DataRow row, IDbCommand command, StatementType statementType,
                                         DataTableMapping tableMapping) : base(row, command, statementType, tableMapping)
        {
        }
    }

    /// <summary>
    /// Provides data for the RowUpdated event. This class cannot be inherited.
    /// </summary>
    public sealed class MySqlRowUpdatedEventArgs : RowUpdatedEventArgs
    {
        /// <summary>
        /// Gets or sets the MySqlCommand executed when Update is called.
        /// </summary>
        public new MySqlCommand Command
        {
            get { return (MySqlCommand)base.Command; }
        }

        /// <summary>
        /// Initializes a new instance of the MySqlRowUpdatedEventArgs class.
        /// </summary>
        /// <param name="row">The <see cref="DataRow"/> sent through an <see cref="DbDataAdapter.Update(DataSet)"/>.</param>
        /// <param name="command">The <see cref="IDbCommand"/> executed when <see cref="DbDataAdapter.Update(DataSet)"/> is called.</param>
        /// <param name="statementType">One of the <see cref="StatementType"/> values that specifies the type of query executed.</param>
        /// <param name="tableMapping">The <see cref="DataTableMapping"/> sent through an <see cref="DbDataAdapter.Update(DataSet)"/>.</param>
        public MySqlRowUpdatedEventArgs(DataRow row, IDbCommand command, StatementType statementType,
                                        DataTableMapping tableMapping) : base(row, command, statementType, tableMapping)
        {
        }
    }
}