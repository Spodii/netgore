using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    /// <summary>
    /// Summary description for MySqlUInt64.
    /// </summary>
    struct MySqlBit : IMySqlValue
    {
        byte[] buffer;
        bool isNull;
        ulong mValue;

        public MySqlBit(bool isnull)
        {
            mValue = 0;
            isNull = isnull;
            buffer = new byte[8];
        }

        public static void SetDSInfo(DataTable dsTable)
        {
            // we use name indexing because this method will only be called
            // when GetSchema is called for the DataSourceInformation 
            // collection and then it wil be cached.
            DataRow row = dsTable.NewRow();
            row["TypeName"] = "BIT";
            row["ProviderDbType"] = MySqlDbType.Bit;
            row["ColumnSize"] = 64;
            row["CreateFormat"] = "BIT";
            row["CreateParameters"] = null;
            row["DataType"] = typeof(UInt64).ToString();
            row["IsAutoincrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = true;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = false;
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
            get { return MySqlDbType.Bit; }
        }

        DbType IMySqlValue.DbType
        {
            get { return DbType.UInt64; }
        }

        object IMySqlValue.Value
        {
            get { return mValue; }
        }

        Type IMySqlValue.SystemType
        {
            get { return typeof(UInt64); }
        }

        string IMySqlValue.MySqlTypeName
        {
            get { return "BIT"; }
        }

        public void WriteValue(MySqlStream stream, bool binary, object value, int length)
        {
            ulong v = Convert.ToUInt64(value);
            if (binary)
                stream.Write(BitConverter.GetBytes(v));
            else
                stream.WriteStringNoNull(v.ToString());
        }

        public IMySqlValue ReadValue(MySqlStream stream, long length, bool isNull)
        {
            this.isNull = isNull;
            if (isNull)
                return this;

            if (buffer == null)
                buffer = new byte[8];
            if (length == -1)
                length = stream.ReadFieldLength();
            Array.Clear(buffer, 0, buffer.Length);
            for (long i = length - 1; i >= 0; i--)
            {
                buffer[i] = (byte)stream.ReadByte();
            }
            mValue = BitConverter.ToUInt64(buffer, 0);
            return this;
        }

        public void SkipValue(MySqlStream stream)
        {
            long len = stream.ReadFieldLength();
            stream.SkipBytes((int)len);
        }

        #endregion
    }
}