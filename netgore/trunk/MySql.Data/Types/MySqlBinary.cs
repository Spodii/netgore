using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    struct MySqlBinary : IMySqlValue
    {
        internal bool IsGuid;
        readonly bool isNull;
        readonly byte[] mValue;
        readonly MySqlDbType type;

        public byte[] Value
        {
            get { return mValue; }
        }

        public MySqlBinary(MySqlDbType type, bool isNull)
        {
            this.type = type;
            this.isNull = isNull;
            mValue = null;
            IsGuid = false;
        }

        public MySqlBinary(MySqlDbType type, byte[] val)
        {
            this.type = type;
            isNull = false;
            mValue = val;
            IsGuid = false;
        }

        static void EscapeByteArray(byte[] bytes, int length, MySqlStream stream)
        {
            //	System.IO.MemoryStream ms = (System.IO.MemoryStream)stream.Stream;
            //	ms.Capacity += (length * 2);

            for (int x = 0; x < length; x++)
            {
                byte b = bytes[x];
                if (b == '\0')
                {
                    stream.WriteByte((byte)'\\');
                    stream.WriteByte((byte)'0');
                }

                else if (b == '\\' || b == '\'' || b == '\"')
                {
                    stream.WriteByte((byte)'\\');
                    stream.WriteByte(b);
                }
                else
                    stream.WriteByte(b);
            }
        }

        public static void SetDSInfo(DataTable dsTable)
        {
            var types = new string[] { "BLOB", "TINYBLOB", "MEDIUMBLOB", "LONGBLOB" };
            var dbtype = new MySqlDbType[]
                         { MySqlDbType.Blob, MySqlDbType.TinyBlob, MySqlDbType.MediumBlob, MySqlDbType.LongBlob };

            // we use name indexing because this method will only be called
            // when GetSchema is called for the DataSourceInformation 
            // collection and then it wil be cached.
            for (int x = 0; x < types.Length; x++)
            {
                DataRow row = dsTable.NewRow();
                row["TypeName"] = types[x];
                row["ProviderDbType"] = dbtype[x];
                row["ColumnSize"] = 0;
                row["CreateFormat"] = types[x];
                row["CreateParameters"] = null;
                row["DataType"] = "System.Byte[]";
                row["IsAutoincrementable"] = false;
                row["IsBestMatch"] = true;
                row["IsCaseSensitive"] = false;
                row["IsFixedLength"] = false;
                row["IsFixedPrecisionScale"] = true;
                row["IsLong"] = true;
                row["IsNullable"] = true;
                row["IsSearchable"] = true;
                row["IsSearchableWithLike"] = true;
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
        }

        #region IMySqlValue Members

        public bool IsNull
        {
            get { return isNull; }
        }

        MySqlDbType IMySqlValue.MySqlDbType
        {
            get { return type; }
        }

        DbType IMySqlValue.DbType
        {
            get { return IsGuid ? DbType.Guid : DbType.Binary; }
        }

        object IMySqlValue.Value
        {
            get
            {
                if (IsGuid)
                    return new Guid(mValue);
                else
                    return mValue;
            }
        }

        Type IMySqlValue.SystemType
        {
            get { return IsGuid ? typeof(Guid) : typeof(byte[]); }
        }

        string IMySqlValue.MySqlTypeName
        {
            get
            {
                switch (type)
                {
                    case MySqlDbType.TinyBlob:
                        return "TINY_BLOB";
                    case MySqlDbType.MediumBlob:
                        return "MEDIUM_BLOB";
                    case MySqlDbType.LongBlob:
                        return "LONG_BLOB";
                    default:
                        return "BLOB";
                }
            }
        }

        void IMySqlValue.WriteValue(MySqlStream stream, bool binary, object val, int length)
        {
            byte[] buffToWrite;

            if (val is Byte[])
                buffToWrite = (byte[])val;
            else if (val is Char[])
                buffToWrite = stream.Encoding.GetBytes(val as char[]);
            else
            {
                string s = val.ToString();
                if (length == 0)
                    length = s.Length;
                else
                    s = s.Substring(0, length);
                buffToWrite = stream.Encoding.GetBytes(s);
            }

            // we assume zero length means write all of the value
            if (length == 0)
                length = buffToWrite.Length;

            if (buffToWrite == null)
                throw new MySqlException("Only byte arrays and strings can be serialized by MySqlBinary");

            if (binary)
            {
                stream.WriteLength(length);
                stream.Write(buffToWrite, 0, length);
            }
            else
            {
                if (stream.Version.isAtLeast(4, 1, 0))
                    stream.WriteStringNoNull("_binary ");

                stream.WriteByte((byte)'\'');
                EscapeByteArray(buffToWrite, length, stream);
                stream.WriteByte((byte)'\'');
            }
        }

        IMySqlValue IMySqlValue.ReadValue(MySqlStream stream, long length, bool nullVal)
        {
            MySqlBinary b;
            if (nullVal)
                b = new MySqlBinary(type, true);
            else
            {
                if (length == -1)
                    length = stream.ReadFieldLength();

                var newBuff = new byte[length];
                stream.Read(newBuff, 0, (int)length);
                b = new MySqlBinary(type, newBuff);
            }
            b.IsGuid = IsGuid;
            return b;
        }

        void IMySqlValue.SkipValue(MySqlStream stream)
        {
            long len = stream.ReadFieldLength();
            stream.SkipBytes((int)len);
        }

        #endregion
    }
}