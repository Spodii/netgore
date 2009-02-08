using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using MySql.Data.Types;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Represents a parameter to a <see cref="MySqlCommand"/>, and optionally, its mapping to <see cref="DataSet"/> columns. This class cannot be inherited.
    /// </summary>
    [TypeConverter(typeof(MySqlParameterConverter))]
    public sealed class MySqlParameter : DbParameter, IDbDataParameter, ICloneable
    {
        const int UNSIGNED_MASK = 0x8000;
        MySqlParameterCollection collection;
        DbType dbType;
        ParameterDirection direction = ParameterDirection.Input;
        bool inferType;
        bool isNullable = false;
        MySqlDbType mySqlDbType;
        string paramName;
        object paramValue;
        byte precision;
        byte scale;
        int size;
        string sourceColumn;
        bool sourceColumnNullMapping;
        DataRowVersion sourceVersion = DataRowVersion.Current;

        internal MySqlParameterCollection Collection
        {
            get { return collection; }
            set { collection = value; }
        }

        /// <summary>
        /// Gets or sets the MySqlDbType of the parameter.
        /// </summary>
        [Category("Data")]
        [DbProviderSpecificTypeProperty(true)]
        public MySqlDbType MySqlDbType
        {
            get { return mySqlDbType; }
            set
            {
                SetMySqlDbType(value);
                inferType = false;
            }
        }

        /// <summary>
        /// Sets or gets a value which indicates whether the source column is nullable. 
        /// This allows <see cref="DbCommandBuilder"/> to correctly generate Update statements 
        /// for nullable columns. 
        /// </summary>
        public override bool SourceColumnNullMapping
        {
            get { return sourceColumnNullMapping; }
            set { sourceColumnNullMapping = value; }
        }

        internal bool TypeHasBeenSet
        {
            get { return inferType == false; }
        }

        /// <summary>
        /// Initializes a new instance of the MySqlParameter class.
        /// </summary>
        public MySqlParameter()
        {
            inferType = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlParameter"/> class with the parameter name and a value of the new MySqlParameter.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map. </param>
        /// <param name="value">An <see cref="Object"/> that is the value of the <see cref="MySqlParameter"/>. </param>
        public MySqlParameter(string parameterName, object value) : this()
        {
            ParameterName = parameterName;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlParameter"/> class with the parameter name and the data type.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map. </param>
        /// <param name="dbType">One of the <see cref="MySqlDbType"/> values. </param>
        public MySqlParameter(string parameterName, MySqlDbType dbType) : this(parameterName, null)
        {
            MySqlDbType = dbType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlParameter"/> class with the parameter name, the <see cref="MySqlDbType"/>, and the size.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map. </param>
        /// <param name="dbType">One of the <see cref="MySqlDbType"/> values. </param>
        /// <param name="size">The length of the parameter. </param>
        public MySqlParameter(string parameterName, MySqlDbType dbType, int size) : this(parameterName, dbType)
        {
            this.size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlParameter"/> class with the parameter name, the <see cref="MySqlDbType"/>, the size, and the source column name.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map. </param>
        /// <param name="dbType">One of the <see cref="MySqlDbType"/> values. </param>
        /// <param name="size">The length of the parameter. </param>
        /// <param name="sourceColumn">The name of the source column. </param>
        public MySqlParameter(string parameterName, MySqlDbType dbType, int size, string sourceColumn)
            : this(parameterName, dbType)
        {
            this.size = size;
            direction = ParameterDirection.Input;
            this.sourceColumn = sourceColumn;
            sourceVersion = DataRowVersion.Current;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlParameter"/> class with the parameter name, the type of the parameter, the size of the parameter, a <see cref="ParameterDirection"/>, the precision of the parameter, the scale of the parameter, the source column, a <see cref="DataRowVersion"/> to use, and the value of the parameter.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map. </param>
        /// <param name="dbType">One of the <see cref="MySqlDbType"/> values. </param>
        /// <param name="size">The length of the parameter. </param>
        /// <param name="direction">One of the <see cref="ParameterDirection"/> values. </param>
        /// <param name="isNullable">true if the value of the field can be null, otherwise false. </param>
        /// <param name="precision">The total number of digits to the left and right of the decimal point to which <see cref="MySqlParameter.Value"/> is resolved.</param>
        /// <param name="scale">The total number of decimal places to which <see cref="MySqlParameter.Value"/> is resolved. </param>
        /// <param name="sourceColumn">The name of the source column. </param>
        /// <param name="sourceVersion">One of the <see cref="DataRowVersion"/> values. </param>
        /// <param name="value">An <see cref="Object"/> that is the value of the <see cref="MySqlParameter"/>. </param>
        /// <exception cref="ArgumentException"/>
        public MySqlParameter(string parameterName, MySqlDbType dbType, int size, ParameterDirection direction, bool isNullable,
                              byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
            : this(parameterName, dbType, size, sourceColumn)
        {
            this.direction = direction;
            this.sourceVersion = sourceVersion;
            Value = value;
        }

        internal MySqlParameter(string name, MySqlDbType type, ParameterDirection dir, string col, DataRowVersion ver, object val)
            : this(name, type)
        {
            direction = dir;
            sourceColumn = col;
            sourceVersion = ver;
            Value = val;
        }

        internal long EstimatedSize()
        {
            if (Value == null || Value == DBNull.Value)
                return 4; // size of NULL
            if (Value is byte[])
                return (Value as byte[]).Length;
            if (Value is string)
                return (Value as string).Length * 4; // account for UTF-8 (yeah I know)
            if (Value is decimal || Value is float)
                return 64;
            return 32;
        }

        internal int GetPSType()
        {
            switch (mySqlDbType)
            {
                case MySqlDbType.Bit:
                    return (int)MySqlDbType.Int64 | UNSIGNED_MASK;
                case MySqlDbType.UByte:
                    return (int)MySqlDbType.Byte | UNSIGNED_MASK;
                case MySqlDbType.UInt64:
                    return (int)MySqlDbType.Int64 | UNSIGNED_MASK;
                case MySqlDbType.UInt32:
                    return (int)MySqlDbType.Int32 | UNSIGNED_MASK;
                case MySqlDbType.UInt24:
                    return (int)MySqlDbType.Int32 | UNSIGNED_MASK;
                case MySqlDbType.UInt16:
                    return (int)MySqlDbType.Int16 | UNSIGNED_MASK;
                default:
                    return (int)mySqlDbType;
            }
        }

        /// <summary>
        /// Resets the <b>DbType</b> property to its original settings. 
        /// </summary>
        public override void ResetDbType()
        {
            inferType = true;
        }

        internal void Serialize(MySqlStream stream, bool binary)
        {
            IMySqlValue v = MySqlField.GetIMySqlValue(mySqlDbType);

            if (!binary && (paramValue == null || paramValue == DBNull.Value))
                stream.WriteStringNoNull("NULL");
            else
                v.WriteValue(stream, binary, paramValue, size);
        }

        void SetDbType(DbType db_type)
        {
            dbType = db_type;
            switch (dbType)
            {
                case DbType.Guid:
                case DbType.AnsiString:
                case DbType.String:
                    mySqlDbType = MySqlDbType.VarChar;
                    break;

                case DbType.AnsiStringFixedLength:
                case DbType.StringFixedLength:
                    mySqlDbType = MySqlDbType.String;
                    break;

                case DbType.Boolean:
                case DbType.Byte:
                    mySqlDbType = MySqlDbType.UByte;
                    break;

                case DbType.SByte:
                    mySqlDbType = MySqlDbType.Byte;
                    break;

                case DbType.Date:
                    mySqlDbType = MySqlDbType.Date;
                    break;
                case DbType.DateTime:
                    mySqlDbType = MySqlDbType.DateTime;
                    break;

                case DbType.Time:
                    mySqlDbType = MySqlDbType.Time;
                    break;
                case DbType.Single:
                    mySqlDbType = MySqlDbType.Float;
                    break;
                case DbType.Double:
                    mySqlDbType = MySqlDbType.Double;
                    break;

                case DbType.Int16:
                    mySqlDbType = MySqlDbType.Int16;
                    break;
                case DbType.UInt16:
                    mySqlDbType = MySqlDbType.UInt16;
                    break;

                case DbType.Int32:
                    mySqlDbType = MySqlDbType.Int32;
                    break;
                case DbType.UInt32:
                    mySqlDbType = MySqlDbType.UInt32;
                    break;

                case DbType.Int64:
                    mySqlDbType = MySqlDbType.Int64;
                    break;
                case DbType.UInt64:
                    mySqlDbType = MySqlDbType.UInt64;
                    break;

                case DbType.Decimal:
                case DbType.Currency:
                    mySqlDbType = MySqlDbType.Decimal;
                    break;

                default:
                    mySqlDbType = MySqlDbType.Blob;
                    break;
            }
        }

        void SetMySqlDbType(MySqlDbType mysql_dbtype)
        {
            mySqlDbType = mysql_dbtype;
            switch (mySqlDbType)
            {
                case MySqlDbType.Decimal:
                    dbType = DbType.Decimal;
                    break;
                case MySqlDbType.Byte:
                    dbType = DbType.SByte;
                    break;
                case MySqlDbType.UByte:
                    dbType = DbType.Byte;
                    break;
                case MySqlDbType.Int16:
                    dbType = DbType.Int16;
                    break;
                case MySqlDbType.UInt16:
                    dbType = DbType.UInt16;
                    break;
                case MySqlDbType.Int24:
                case MySqlDbType.Int32:
                    dbType = DbType.Int32;
                    break;
                case MySqlDbType.UInt24:
                case MySqlDbType.UInt32:
                    dbType = DbType.UInt32;
                    break;
                case MySqlDbType.Int64:
                    dbType = DbType.Int64;
                    break;
                case MySqlDbType.UInt64:
                    dbType = DbType.UInt64;
                    break;
                case MySqlDbType.Bit:
                    dbType = DbType.UInt64;
                    break;
                case MySqlDbType.Float:
                    dbType = DbType.Single;
                    break;
                case MySqlDbType.Double:
                    dbType = DbType.Double;
                    break;
                case MySqlDbType.Timestamp:
                case MySqlDbType.DateTime:
                    dbType = DbType.DateTime;
                    break;
                case MySqlDbType.Date:
                case MySqlDbType.Newdate:
                case MySqlDbType.Year:
                    dbType = DbType.Date;
                    break;
                case MySqlDbType.Time:
                    dbType = DbType.Time;
                    break;
                case MySqlDbType.Enum:
                case MySqlDbType.Set:
                case MySqlDbType.VarChar:
                    dbType = DbType.String;
                    break;
                case MySqlDbType.TinyBlob:
                case MySqlDbType.MediumBlob:
                case MySqlDbType.LongBlob:
                case MySqlDbType.Blob:
                    dbType = DbType.Object;
                    break;
                case MySqlDbType.String:
                    dbType = DbType.StringFixedLength;
                    break;
            }
        }

        void SetTypeFromValue()
        {
            if (paramValue == null || paramValue == DBNull.Value)
                return;

            if (paramValue is Guid)
                DbType = DbType.String;
            else if (paramValue is TimeSpan)
                DbType = DbType.Time;
            else if (paramValue is bool)
                DbType = DbType.Byte;
            else
            {
                TypeCode tc = Type.GetTypeCode(paramValue.GetType());
                switch (tc)
                {
                    case TypeCode.SByte:
                        DbType = DbType.SByte;
                        break;
                    case TypeCode.Byte:
                        DbType = DbType.Byte;
                        break;
                    case TypeCode.Int16:
                        DbType = DbType.Int16;
                        break;
                    case TypeCode.UInt16:
                        DbType = DbType.UInt16;
                        break;
                    case TypeCode.Int32:
                        DbType = DbType.Int32;
                        break;
                    case TypeCode.UInt32:
                        DbType = DbType.UInt32;
                        break;
                    case TypeCode.Int64:
                        DbType = DbType.Int64;
                        break;
                    case TypeCode.UInt64:
                        DbType = DbType.UInt64;
                        break;
                    case TypeCode.DateTime:
                        DbType = DbType.DateTime;
                        break;
                    case TypeCode.String:
                        DbType = DbType.String;
                        break;
                    case TypeCode.Single:
                        DbType = DbType.Single;
                        break;
                    case TypeCode.Double:
                        DbType = DbType.Double;
                        break;
                    case TypeCode.Decimal:
                        DbType = DbType.Decimal;
                        break;
                    default:
                        DbType = DbType.Object;
                        break;
                }
            }
        }

        /// <summary>
        /// Overridden. Gets a string containing the <see cref="ParameterName"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return paramName;
        }

        #region ICloneable Members

        object ICloneable.Clone()
        {
            MySqlParameter clone = new MySqlParameter(paramName, mySqlDbType, direction, sourceColumn, sourceVersion, paramValue)
                                   { inferType = inferType };
            // if we have not had our type set yet then our clone should not either
            return clone;
        }

        #endregion

        #region IDbDataParameter Members

        /// <summary>
        /// Gets or sets the <see cref="DbType"/> of the parameter.
        /// </summary>
        public override DbType DbType
        {
            get { return dbType; }
            set
            {
                SetDbType(value);
                inferType = false;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter is input-only, output-only, bidirectional, or a stored procedure return value parameter.
        /// As of MySql version 4.1 and earlier, input-only is the only valid choice.
        /// </summary>
        [Category("Data")]
        public override ParameterDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter accepts null values.
        /// </summary>
        [Browsable(false)]
        public override Boolean IsNullable
        {
            get { return isNullable; }
            set { isNullable = value; }
        }

        /// <summary>
        /// Gets or sets the name of the MySqlParameter.
        /// </summary>
        [Category("Misc")]
        public override String ParameterName
        {
            get { return paramName; }
            set
            {
                if (collection != null)
                    collection.ParameterNameChanged(this, paramName, value);
                paramName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the source column that is mapped to the <see cref="DataSet"/> and used for loading or returning the <see cref="Value"/>.
        /// </summary>
        [Category("Data")]
        public override String SourceColumn
        {
            get { return sourceColumn; }
            set { sourceColumn = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="DataRowVersion"/> to use when loading <see cref="Value"/>.
        /// </summary>
        [Category("Data")]
        public override DataRowVersion SourceVersion
        {
            get { return sourceVersion; }
            set { sourceVersion = value; }
        }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        [TypeConverter(typeof(StringConverter))]
        [Category("Data")]
        public override object Value
        {
            get { return paramValue; }
            set
            {
                paramValue = value;
                if (value is Byte[])
                    size = (value as Byte[]).Length;
                else if (value is String)
                    size = (value as string).Length;
                if (inferType)
                    SetTypeFromValue();
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of digits used to represent the <see cref="Value"/> property.
        /// </summary>
        [Category("Data")]
        public byte Precision
        {
            get { return precision; }
            set { precision = value; }
        }

        /// <summary>
        /// Gets or sets the number of decimal places to which <see cref="Value"/> is resolved.
        /// </summary>
        [Category("Data")]
        public byte Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// Gets or sets the maximum size, in bytes, of the data within the column.
        /// </summary>
        [Category("Data")]
        public override int Size
        {
            get { return size; }
            set { size = value; }
        }

        #endregion
    }

    class MySqlParameterConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
                return true;

            // Always call the base to see if it can perform the conversion.
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                ConstructorInfo ci =
                    typeof(MySqlParameter).GetConstructor(new Type[]
                                                          {
                                                              typeof(string), typeof(MySqlDbType), typeof(int),
                                                              typeof(ParameterDirection), typeof(bool), typeof(byte), typeof(byte),
                                                              typeof(string), typeof(DataRowVersion), typeof(object)
                                                          });
                MySqlParameter p = (MySqlParameter)value;
                return new InstanceDescriptor(ci,
                                              new object[]
                                              {
                                                  p.ParameterName, p.DbType, p.Size, p.Direction, p.IsNullable, p.Precision, p.Scale
                                                  , p.SourceColumn, p.SourceVersion, p.Value
                                              });
            }

            // Always call base, even if you can't convert.
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}