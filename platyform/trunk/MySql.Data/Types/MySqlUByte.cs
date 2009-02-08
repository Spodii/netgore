using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    struct MySqlUByte : IMySqlValue
    {
        readonly bool isNull;
        readonly byte mValue;

        public byte Value
        {
            get { return mValue; }
        }

        public MySqlUByte(bool isNull)
        {
            this.isNull = isNull;
            mValue = 0;
        }

        public MySqlUByte(byte val)
        {
            isNull = false;
            mValue = val;
        }

        internal static void SetDSInfo(DataTable dsTable)
        {
            // we use name indexing because this method will only be called
            // when GetSchema is called for the DataSourceInformation 
            // collection and then it wil be cached.
            DataRow row = dsTable.NewRow();
            row["TypeName"] = "TINY INT";
            row["ProviderDbType"] = MySqlDbType.UByte;
            row["ColumnSize"] = 0;
            row["CreateFormat"] = "TINYINT UNSIGNED";
            row["CreateParameters"] = null;
            row["DataType"] = "System.Byte";
            row["IsAutoincrementable"] = true;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = true;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = true;
            row["MaximumScale"] = 0;
            row["MinimumScale"] = 0;
            row["IsConcurrencyType"] = DBNull.Value;
            row["IsLiteralsSupported"] = false;
            row["LiteralPrefix"] = null;
            row["LiteralSuffix"] = null;
            row["NativeDataType"] = null;
            dsTable.Rows.Add(row);
        }

        #region IMySqlValue Members

        public bool IsNull
        {
            get { return isNull; }
        }

        MySqlDbType IMySqlValue.MySqlDbType
        {
            get { return MySqlDbType.UByte; }
        }

        DbType IMySqlValue.DbType
        {
            get { return DbType.Byte; }
        }

        object IMySqlValue.Value
        {
            get { return mValue; }
        }

        Type IMySqlValue.SystemType
        {
            get { return typeof(byte); }
        }

        string IMySqlValue.MySqlTypeName
        {
            get { return "TINYINT"; }
        }

        void IMySqlValue.WriteValue(MySqlStream stream, bool binary, object val, int length)
        {
            byte v = ((IConvertible)val).ToByte(null);
            if (binary)
                stream.WriteByte(v);
            else
                stream.WriteStringNoNull(v.ToString());
        }

        IMySqlValue IMySqlValue.ReadValue(MySqlStream stream, long length, bool nullVal)
        {
            if (nullVal)
                return new MySqlUByte(true);

            if (length == -1)
                return new MySqlUByte((byte)stream.ReadByte());
            else
                return new MySqlUByte(Byte.Parse(stream.ReadString(length)));
        }

        void IMySqlValue.SkipValue(MySqlStream stream)
        {
            stream.ReadByte();
        }

        #endregion
    }
}