using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Diagnostics;
using MySql.Data.MySqlClient.Properties;
using MySql.Data.Types;

namespace MySql.Data.MySqlClient
{
    /// <include file='docs/MySqlDataReader.xml' path='docs/ClassSummary/*'/>
    public sealed class MySqlDataReader : DbDataReader, IDataReader
    {
        // The DataReader should always be open when returned to the user.
        internal IMySqlValue[] values;
        readonly Driver driver;
        readonly Hashtable fieldHashCI;
        readonly Hashtable fieldHashCS;
        readonly PreparableStatement statement;
        long affectedRows;
        bool canRead;
        MySqlCommand command;
        CommandBehavior commandBehavior;
        MySqlConnection connection = null;
        MySqlField[] fields;
        bool hasRead;
        bool hasRows;
        bool isOpen = true;
        long lastInsertId;
        bool nextResultDone;
        int seqIndex;
        bool[] uaFieldsUsed;

        internal CommandBehavior Behavior
        {
            get { return commandBehavior; }
        }

        /// <summary>
        /// Gets a value indicating whether the MySqlDataReader contains one or more rows.
        /// </summary>
        public override bool HasRows
        {
            get { return hasRows; }
        }

        internal long InsertedId
        {
            get { return lastInsertId; }
        }

        internal MySqlDataReader(MySqlCommand cmd, PreparableStatement statement, CommandBehavior behavior)
        {
            command = cmd;
            connection = command.Connection;
            commandBehavior = behavior;
            driver = connection.driver;
            affectedRows = -1;
            this.statement = statement;
            nextResultDone = false;
            fieldHashCS = new Hashtable();
            fieldHashCI = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
        }

        void ClearCurrentResultset()
        {
            if (!canRead)
                return;
            while (driver.SkipDataRow())
            {
            }

            if (!connection.Settings.UseUsageAdvisor)
                return;

            // we were asked to run the usage advisor so report if the resultset
            // was not entirely read.
            if (canRead)
                connection.UsageAdvisor.ReadPartialResultSet(command.CommandText);

            bool readAll = true;
            foreach (bool b in uaFieldsUsed)
            {
                readAll &= b;
            }
            if (!readAll)
                connection.UsageAdvisor.ReadPartialRowSet(command.CommandText, uaFieldsUsed, fields);
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetBoolean(string name)
        {
            return GetBoolean(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a byte.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte GetByte(string name)
        {
            return GetByte(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a single character.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public char GetChar(string name)
        {
            return GetChar(GetOrdinal(name));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetDateTimeS/*'/>
        public DateTime GetDateTime(string column)
        {
            return GetDateTime(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetDecimalS/*'/>
        public Decimal GetDecimal(string column)
        {
            return GetDecimal(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetDoubleS/*'/>
        public double GetDouble(string column)
        {
            return GetDouble(GetOrdinal(column));
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator"/> that iterates through the <see cref="MySqlDataReader"/>. 
        /// </summary>
        /// <returns></returns>
        public override IEnumerator GetEnumerator()
        {
            return new DbEnumerator(this, (commandBehavior & CommandBehavior.CloseConnection) != 0);
        }

        IMySqlValue GetFieldValue(int index, bool checkNull)
        {
            if (index < 0 || index >= fields.Length)
                throw new ArgumentException("You have specified an invalid column ordinal.");

            if (!hasRead)
                throw new MySqlException("Invalid attempt to access a field before calling Read()");

            // keep count of how many columns we have left to access
            uaFieldsUsed[index] = true;

            if ((commandBehavior & CommandBehavior.SequentialAccess) != 0 && index != seqIndex)
            {
                if (index < seqIndex)
                    throw new MySqlException("Invalid attempt to read a prior column using SequentialAccess");
                while (seqIndex < (index - 1))
                {
                    driver.SkipColumnValue(values[++seqIndex]);
                }
                values[index] = driver.ReadColumnValue(index, fields[index], values[index]);
                seqIndex = index;
            }

            IMySqlValue v = values[index];
            if (checkNull && v.IsNull)
                throw new SqlNullValueException();

            return v;
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetFloatS/*'/>
        public float GetFloat(string column)
        {
            return GetFloat(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetGuidS/*'/>
        public Guid GetGuid(string column)
        {
            return GetGuid(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetInt16S/*'/>
        public Int16 GetInt16(string column)
        {
            return GetInt16(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetInt32S/*'/>
        public Int32 GetInt32(string column)
        {
            return GetInt32(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetInt64S/*'/>
        public Int64 GetInt64(string column)
        {
            return GetInt64(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetMySqlDateTime/*'/>
        public MySqlDateTime GetMySqlDateTime(string column)
        {
            return GetMySqlDateTime(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetMySqlDateTime/*'/>
        public MySqlDateTime GetMySqlDateTime(int column)
        {
            return (MySqlDateTime)GetFieldValue(column, true);
        }

        /// <summary>
        /// GetResultSet is the core resultset processing method.  It gets called by NextResult
        /// and will loop until it finds a select resultset at which point it will return the 
        /// number of columns in that result.  It will _not_ return for non-select resultsets instead
        /// just updating the internal lastInsertId and affectedRows variables.  
        /// </summary>
        /// <returns>-1 if no more results exist, >= 0 for select results</returns>
        long GetResultSet()
        {
            while (true)
            {
                ulong affectedRowsTemp = 0;
                long fieldCount = driver.ReadResult(ref affectedRowsTemp, ref lastInsertId);
                if (fieldCount > 0)
                    return fieldCount;
                else if (fieldCount == 0)
                {
                    command.lastInsertedId = lastInsertId;
                    if (affectedRows == -1)
                        affectedRows = (long)affectedRowsTemp;
                    else
                        affectedRows += (long)affectedRowsTemp;
                }
                else if (fieldCount == -1)
                {
                    if (!statement.ExecuteNext())
                        break;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the value of the specified column as a sbyte.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte GetSByte(string name)
        {
            return GetByte(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a sbyte.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public sbyte GetSByte(int i)
        {
            IMySqlValue v = GetFieldValue(i, false);
            if (v is MySqlByte)
                return ((MySqlByte)v).Value;
            else
                return ((MySqlByte)v).Value;
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetStringS/*'/>
        public string GetString(string column)
        {
            return GetString(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetTimeSpan/*'/>
        public TimeSpan GetTimeSpan(string column)
        {
            return GetTimeSpan(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetTimeSpan/*'/>
        public TimeSpan GetTimeSpan(int column)
        {
            IMySqlValue val = GetFieldValue(column, true);

            MySqlTimeSpan ts = (MySqlTimeSpan)val;
            return ts.Value;
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetUInt16/*'/>
        public UInt16 GetUInt16(string column)
        {
            return GetUInt16(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetUInt16/*'/>
        public UInt16 GetUInt16(int column)
        {
            IMySqlValue v = GetFieldValue(column, true);
            if (v is MySqlUInt16)
                return ((MySqlUInt16)v).Value;

            connection.UsageAdvisor.Converting(command.CommandText, fields[column].ColumnName, v.MySqlTypeName, "UInt16");
            return Convert.ToUInt16(v.Value);
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetUInt32/*'/>
        public UInt32 GetUInt32(string column)
        {
            return GetUInt32(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetUInt32/*'/>
        public UInt32 GetUInt32(int column)
        {
            IMySqlValue v = GetFieldValue(column, true);
            if (v is MySqlUInt32)
                return ((MySqlUInt32)v).Value;

            connection.UsageAdvisor.Converting(command.CommandText, fields[column].ColumnName, v.MySqlTypeName, "UInt32");
            return Convert.ToUInt32(v.Value);
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetUInt64/*'/>
        public UInt64 GetUInt64(string column)
        {
            return GetUInt64(GetOrdinal(column));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetUInt64/*'/>
        public UInt64 GetUInt64(int column)
        {
            IMySqlValue v = GetFieldValue(column, true);
            if (v is MySqlUInt64)
                return ((MySqlUInt64)v).Value;

            connection.UsageAdvisor.Converting(command.CommandText, fields[column].ColumnName, v.MySqlTypeName, "UInt64");
            return Convert.ToUInt64(v.Value);
        }

        #region IDataReader Members

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.  This method is not 
        /// supported currently and always returns 0.
        /// </summary>
        public override int Depth
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        public override int FieldCount
        {
            // Return the count of the number of columns, which in
            // this case is the size of the column metadata
            // array.
            get
            {
                if (fields != null)
                    return fields.Length;
                return 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the data reader is closed.
        /// </summary>
        public override bool IsClosed
        {
            get { return !isOpen; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        public override int RecordsAffected
        {
            // RecordsAffected returns the number of rows affected in batch
            // statments from insert/delete/update statments.  This property
            // is not completely accurate until .Close() has been called.
            get { return (int)affectedRows; }
        }

        /// <summary>
        /// Overloaded. Gets the value of a column in its native format.
        /// In C#, this property is the indexer for the MySqlDataReader class.
        /// </summary>
        public override object this[int i]
        {
            get { return GetValue(i); }
        }

        /// <summary>
        /// Gets the value of a column in its native format.
        ///	[C#] In C#, this property is the indexer for the MySqlDataReader class.
        /// </summary>
        public override object this[String name]
        {
            // Look up the ordinal and return 
            // the value at that position.
            get { return this[GetOrdinal(name)]; }
        }

        /// <summary>
        /// Closes the MySqlDataReader object.
        /// </summary>
        public override void Close()
        {
            if (!isOpen)
            {
                if (connection != null)
                    connection.EnsureReleaseReader(this);
                return;
            }

            bool shouldCloseConnection;
            try
            {
                shouldCloseConnection = (commandBehavior & CommandBehavior.CloseConnection) != 0;
                commandBehavior = CommandBehavior.Default;
                connection.Reader = null;

                // we set the nextResultDone var to true inside NextResult when
                // it returns false.  This allows us to avoid calling NextResult
                // here unnecessarily.  Calling NextResult here will work but this
                // is just an optimization.
                if (!nextResultDone)
                {
                    while (NextResult())
                    {
                    }
                }
            }
            finally
            {
                connection.ReleaseReader(this);
            }

            // we now give the command a chance to terminate.  In the case of
            // stored procedures it needs to update out and inout parameters
            command.Close();

            if (shouldCloseConnection)
                connection.Close();

            command = null;
            connection = null;

            isOpen = false;
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override bool GetBoolean(int i)
        {
            return Convert.ToBoolean(GetValue(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a byte.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override byte GetByte(int i)
        {
            IMySqlValue v = GetFieldValue(i, false);
            if (v is MySqlUByte)
                return ((MySqlUByte)v).Value;
            else
                return (byte)((MySqlByte)v).Value;
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer an array starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="fieldOffset">The index within the field from which to begin the read operation. </param>
        /// <param name="buffer">The buffer into which to read the stream of bytes. </param>
        /// <param name="bufferoffset">The index for buffer to begin the read operation. </param>
        /// <param name="length">The maximum length to copy into the buffer. </param>
        /// <returns>The actual number of bytes read.</returns>
        /// <include file='docs/MySqlDataReader.xml' path='MyDocs/MyMembers[@name="GetBytes"]/*'/>
        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            if (i >= fields.Length)
                throw new IndexOutOfRangeException();

            IMySqlValue val = GetFieldValue(i, false);

            if (!(val is MySqlBinary))
                throw new MySqlException("GetBytes can only be called on binary columns");

            MySqlBinary binary = (MySqlBinary)val;
            if (buffer == null)
                return binary.Value.Length;

            if (bufferoffset >= buffer.Length || bufferoffset < 0)
                throw new IndexOutOfRangeException("Buffer index must be a valid index in buffer");
            if (buffer.Length < (bufferoffset + length))
                throw new ArgumentException("Buffer is not large enough to hold the requested data");
            if (fieldOffset < 0 || ((ulong)fieldOffset >= (ulong)binary.Value.Length && (ulong)binary.Value.Length > 0))
                throw new IndexOutOfRangeException("Data index must be a valid index in the field");

            var bytes = binary.Value;

            // adjust the length so we don't run off the end
            if ((ulong)binary.Value.Length < (ulong)(fieldOffset + length))
                length = (int)((ulong)binary.Value.Length - (ulong)fieldOffset);

            Buffer.BlockCopy(bytes, (int)fieldOffset, buffer, bufferoffset, length);

            return length;
        }

        /// <summary>
        /// Gets the value of the specified column as a single character.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override char GetChar(int i)
        {
            string s = GetString(i);
            return s[0];
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array starting at the given buffer offset.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldoffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            if (i >= fields.Length)
                throw new IndexOutOfRangeException();

            string valAsString = GetString(i);

            if (buffer == null)
                return valAsString.Length;

            if (bufferoffset >= buffer.Length || bufferoffset < 0)
                throw new IndexOutOfRangeException("Buffer index must be a valid index in buffer");
            if (buffer.Length < (bufferoffset + length))
                throw new ArgumentException("Buffer is not large enough to hold the requested data");
            if (fieldoffset < 0 || fieldoffset >= valAsString.Length)
                throw new IndexOutOfRangeException("Field offset must be a valid index in the field");

            if (valAsString.Length < length)
                length = valAsString.Length;
            valAsString.CopyTo((int)fieldoffset, buffer, bufferoffset, length);
            return length;
        }

        /// <summary>
        /// Gets the name of the source data type.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override String GetDataTypeName(int i)
        {
            if (!isOpen)
                throw new Exception("No current query in data reader");
            if (i >= fields.Length)
                throw new IndexOutOfRangeException();

            // return the name of the type used on the backend
            return values[i].MySqlTypeName;
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetDateTime/*'/>
        public override DateTime GetDateTime(int i)
        {
            IMySqlValue val = GetFieldValue(i, true);
            MySqlDateTime dt;

            if (val is MySqlDateTime)
                dt = (MySqlDateTime)val;
            else
            {
                // we need to do this because functions like date_add return string
                string s = GetString(i);
                dt = MySqlDateTime.Parse(s, connection.driver.Version);
            }

            if (connection.Settings.ConvertZeroDateTime && !dt.IsValidDateTime)
                return DateTime.MinValue;
            else
                return dt.GetDateTime();
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetDecimal/*'/>
        public override Decimal GetDecimal(int i)
        {
            IMySqlValue v = GetFieldValue(i, true);
            if (v is MySqlDecimal)
                return ((MySqlDecimal)v).Value;
            return Convert.ToDecimal(v.Value);
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetDouble/*'/>
        public override double GetDouble(int i)
        {
            IMySqlValue v = GetFieldValue(i, true);
            if (v is MySqlDouble)
                return ((MySqlDouble)v).Value;
            return Convert.ToDouble(v.Value);
        }

        /// <summary>
        /// Gets the Type that is the data type of the object.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override Type GetFieldType(int i)
        {
            if (!isOpen)
                throw new Exception("No current query in data reader");
            if (i >= fields.Length)
                throw new IndexOutOfRangeException();

            if (values[i] is MySqlDateTime)
            {
                if (!connection.Settings.AllowZeroDateTime)
                    return typeof(DateTime);
                return typeof(MySqlDateTime);
            }
            return values[i].SystemType;
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetFloat/*'/>
        public override float GetFloat(int i)
        {
            IMySqlValue v = GetFieldValue(i, true);
            if (v is MySqlSingle)
                return ((MySqlSingle)v).Value;
            return Convert.ToSingle(v.Value);
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetGuid/*'/>
        public override Guid GetGuid(int i)
        {
            return new Guid(GetString(i));
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetInt16/*'/>
        public override Int16 GetInt16(int i)
        {
            IMySqlValue v = GetFieldValue(i, true);
            if (v is MySqlInt16)
                return ((MySqlInt16)v).Value;

            connection.UsageAdvisor.Converting(command.CommandText, fields[i].ColumnName, v.MySqlTypeName, "Int16");
            return ((IConvertible)v.Value).ToInt16(null);
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetInt32/*'/>
        public override Int32 GetInt32(int i)
        {
            IMySqlValue v = GetFieldValue(i, true);
            if (v is MySqlInt32)
                return ((MySqlInt32)v).Value;

            connection.UsageAdvisor.Converting(command.CommandText, fields[i].ColumnName, v.MySqlTypeName, "Int32");
            return ((IConvertible)v.Value).ToInt32(null);
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetInt64/*'/>
        public override Int64 GetInt64(int i)
        {
            IMySqlValue v = GetFieldValue(i, true);
            if (v is MySqlInt64)
                return ((MySqlInt64)v).Value;

            connection.UsageAdvisor.Converting(command.CommandText, fields[i].ColumnName, v.MySqlTypeName, "Int64");
            return ((IConvertible)v.Value).ToInt64(null);
        }

        /// <summary>
        /// Gets the name of the specified column.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override String GetName(int i)
        {
            return fields[i].ColumnName;
        }

        /// <summary>
        /// Gets the column ordinal, given the name of the column.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override int GetOrdinal(string name)
        {
            if (!isOpen)
                throw new Exception("No current query in data reader");

            // first we try a quick hash lookup
            object ordinal = fieldHashCS[name];
            if (ordinal != null)
                return (int)ordinal;

            // ok that failed so we use our CI hash
            ordinal = fieldHashCI[name];
            if (ordinal != null)
                return (int)ordinal;

            // Throw an exception if the ordinal cannot be found.
            throw new IndexOutOfRangeException("Could not find specified column in results");
        }

        /// <summary>
        /// Returns a DataTable that describes the column metadata of the MySqlDataReader.
        /// </summary>
        /// <returns></returns>
        public override DataTable GetSchemaTable()
        {
            // Only Results from SQL SELECT Queries 
            // get a DataTable for schema of the result
            // otherwise, DataTable is null reference
            if (fields == null || fields.Length == 0)
                return null;

            DataTable dataTableSchema = new DataTable("SchemaTable");

            dataTableSchema.Columns.Add("ColumnName", typeof(string));
            dataTableSchema.Columns.Add("ColumnOrdinal", typeof(int));
            dataTableSchema.Columns.Add("ColumnSize", typeof(int));
            dataTableSchema.Columns.Add("NumericPrecision", typeof(int));
            dataTableSchema.Columns.Add("NumericScale", typeof(int));
            dataTableSchema.Columns.Add("IsUnique", typeof(bool));
            dataTableSchema.Columns.Add("IsKey", typeof(bool));
            DataColumn dc = dataTableSchema.Columns["IsKey"];
            dc.AllowDBNull = true; // IsKey can have a DBNull
            dataTableSchema.Columns.Add("BaseCatalogName", typeof(string));
            dataTableSchema.Columns.Add("BaseColumnName", typeof(string));
            dataTableSchema.Columns.Add("BaseSchemaName", typeof(string));
            dataTableSchema.Columns.Add("BaseTableName", typeof(string));
            dataTableSchema.Columns.Add("DataType", typeof(Type));
            dataTableSchema.Columns.Add("AllowDBNull", typeof(bool));
            dataTableSchema.Columns.Add("ProviderType", typeof(int));
            dataTableSchema.Columns.Add("IsAliased", typeof(bool));
            dataTableSchema.Columns.Add("IsExpression", typeof(bool));
            dataTableSchema.Columns.Add("IsIdentity", typeof(bool));
            dataTableSchema.Columns.Add("IsAutoIncrement", typeof(bool));
            dataTableSchema.Columns.Add("IsRowVersion", typeof(bool));
            dataTableSchema.Columns.Add("IsHidden", typeof(bool));
            dataTableSchema.Columns.Add("IsLong", typeof(bool));
            dataTableSchema.Columns.Add("IsReadOnly", typeof(bool));

            int ord = 1;
            for (int i = 0; i < fields.Length; i++)
            {
                MySqlField f = fields[i];
                DataRow r = dataTableSchema.NewRow();
                r["ColumnName"] = f.ColumnName;
                r["ColumnOrdinal"] = ord++;
                r["ColumnSize"] = f.IsTextField ? f.ColumnLength / f.MaxLength : f.ColumnLength;
                int prec = f.Precision;
                int pscale = f.Scale;
                if (prec != -1)
                    r["NumericPrecision"] = (short)prec;
                if (pscale != -1)
                    r["NumericScale"] = (short)pscale;
                r["DataType"] = GetFieldType(i);
                r["ProviderType"] = (int)f.Type;
                r["IsLong"] = f.IsBlob && f.ColumnLength > 255;
                r["AllowDBNull"] = f.AllowsNull;
                r["IsReadOnly"] = false;
                r["IsRowVersion"] = false;
                r["IsUnique"] = f.IsUnique;
                r["IsKey"] = f.IsPrimaryKey;
                r["IsAutoIncrement"] = f.IsAutoIncrement;
                r["BaseSchemaName"] = f.DatabaseName;
                r["BaseCatalogName"] = null;
                r["BaseTableName"] = f.RealTableName;
                r["BaseColumnName"] = f.OriginalColumnName;

                dataTableSchema.Rows.Add(r);
            }

            return dataTableSchema;
        }

        /// <include file='docs/MySqlDataReader.xml' path='docs/GetString/*'/>
        public override String GetString(int i)
        {
            IMySqlValue val = GetFieldValue(i, true);

            if (val is MySqlBinary)
            {
                var v = ((MySqlBinary)val).Value;
                return fields[i].Encoding.GetString(v, 0, v.Length);
            }

            return val.Value.ToString();
        }

        /// <summary>
        /// Gets the value of the specified column in its native format.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override object GetValue(int i)
        {
            if (!isOpen)
                throw new Exception("No current query in data reader");
            if (i >= fields.Length)
                throw new IndexOutOfRangeException();

            IMySqlValue val = GetFieldValue(i, false);
            if (val.IsNull)
                return DBNull.Value;

            // if the column is a date/time, then we return a MySqlDateTime
            // so .ToString() will print '0000-00-00' correctly
            if (val is MySqlDateTime)
            {
                MySqlDateTime dt = (MySqlDateTime)val;
                if (!dt.IsValidDateTime && connection.Settings.ConvertZeroDateTime)
                    return DateTime.MinValue;
                else if (connection.Settings.AllowZeroDateTime)
                    return val;
                else
                    return dt.GetDateTime();
            }

            return val.Value;
        }

        /// <summary>
        /// Gets all attribute columns in the collection for the current row.
        /// </summary>
        /// <param name="pvalues"></param>
        /// <returns></returns>
        public override int GetValues(object[] pvalues)
        {
            if (!hasRead)
                return 0;
            int numCols = Math.Min(pvalues.Length, fields.Length);
            for (int i = 0; i < numCols; i++)
            {
                pvalues[i] = GetValue(i);
            }

            return numCols;
        }

        IDataReader IDataRecord.GetData(int i)
        {
            return GetData(i);
        }

        /// <summary>
        /// Gets a value indicating whether the column contains non-existent or missing values.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override bool IsDBNull(int i)
        {
            return DBNull.Value == GetValue(i);
        }

        /// <summary>
        /// Advances the data reader to the next result, when reading the results of batch SQL statements.
        /// </summary>
        /// <returns></returns>
        public override bool NextResult()
        {
            if (!isOpen)
                throw new MySqlException(Resources.NextResultIsClosed);

            bool firstResult = fields == null;
            if (fields != null)
            {
                ClearCurrentResultset();
                fields = null;
            }

            // single result means we only return a single resultset.  If we have already
            // returned one, then we return false;
            if (!firstResult && (commandBehavior & CommandBehavior.SingleResult) != 0)
                return false;

            // tell our command to continue execution of the SQL batch until it its
            // another resultset
            try
            {
                long fieldCount = GetResultSet();
                if (fieldCount == -1)
                {
                    nextResultDone = true;
                    hasRows = canRead = false;
                    return false;
                }

                // issue any requested UA warnings
                if (connection.Settings.UseUsageAdvisor)
                {
                    if ((connection.driver.ServerStatus & ServerStatusFlags.NoIndex) != 0)
                        connection.UsageAdvisor.UsingNoIndex(command.CommandText);
                    if ((connection.driver.ServerStatus & ServerStatusFlags.BadIndex) != 0)
                        connection.UsageAdvisor.UsingBadIndex(command.CommandText);
                }

                fields = driver.ReadColumnMetadata((int)fieldCount);
                fieldHashCS.Clear();
                fieldHashCI.Clear();
                values = new IMySqlValue[fields.Length];
                for (int i = 0; i < fields.Length; i++)
                {
                    string columnName = fields[i].ColumnName;
                    if (!fieldHashCS.ContainsKey(columnName))
                        fieldHashCS.Add(columnName, i);
                    if (!fieldHashCI.ContainsKey(columnName))
                        fieldHashCI.Add(columnName, i);
                    values[i] = fields[i].GetValueObject();
                }
                hasRead = false;

                uaFieldsUsed = new bool[fields.Length];
                hasRows = canRead = driver.FetchDataRow(statement.StatementId, 0, fields.Length);
                return true;
            }
            catch (MySqlException ex)
            {
                if (ex.IsFatal)
                    connection.Abort();
                nextResultDone = true;
                hasRows = canRead = false;
                if (command.TimedOut)
                    throw new MySqlException(Resources.Timeout);
                if (ex.Number == 0)
                    throw new MySqlException(Resources.FatalErrorReadingResult, ex);
                throw;
            }
        }

        /// <summary>
        /// Advances the MySqlDataReader to the next record.
        /// </summary>
        /// <returns></returns>
        public override bool Read()
        {
            if (!isOpen)
                throw new MySqlException("Invalid attempt to Read when reader is closed.");

            if (!canRead)
                return false;

            if (Behavior == CommandBehavior.SingleRow && hasRead)
                return false;

            try
            {
                bool isSequential = (Behavior & CommandBehavior.SequentialAccess) != 0;
                seqIndex = -1;
                if (hasRead)
                    canRead = driver.FetchDataRow(statement.StatementId, 0, fields.Length);
                hasRead = true;
                if (canRead && !isSequential)
                {
                    for (int i = 0; i < fields.Length; i++)
                    {
                        values[i] = driver.ReadColumnValue(i, fields[i], values[i]);
                    }
                }

                return canRead;
            }
            catch (MySqlException ex)
            {
                if (ex.IsFatal)
                    connection.Abort();

                // if we get a query interrupted then our resultset is done
                if (ex.Number == 1317)
                {
                    nextResultDone = true;
                    canRead = false;
                    if (command.TimedOut)
                        throw new MySqlException(Resources.Timeout);
                    return false;
                }

                throw new MySqlException(Resources.FatalErrorDuringRead, ex);
            }
        }

        #endregion
    }
}