using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    struct MySqlByte : IMySqlValue
    {
        readonly bool isNull;
        sbyte mValue;
        bool treatAsBool;

        internal bool TreatAsBoolean
        {
            get { return treatAsBool; }
            set { treatAsBool = value; }
        }

        public sbyte Value
        {
            get { return mValue; }
            set { mValue = value; }
        }

        public MySqlByte(bool isNull)
        {
            this.isNull = isNull;
            mValue = 0;
            treatAsBool = false;
        }

        public MySqlByte(sbyte val)
        {
            isNull = false;
            mValue = val;
            treatAsBool = false;
        }

        internal static void SetDSInfo(DataTable dsTable)
        {
            // we use name indexing because this method will only be called
            // when GetSchema is called for the DataSourceInformation 
            // collection and then it wil be cached.
            DataRow row = dsTable.NewRow();
            row["TypeName"] = "TINYINT";
            row["ProviderDbType"] = MySqlDbType.Byte;
            row["ColumnSize"] = 0;
            row["CreateFormat"] = "TINYINT";
            row["CreateParameters"] = null;
            row["DataType"] = "System.SByte";
            row["IsAutoincrementable"] = true;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
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
            get { return MySqlDbType.Byte; }
        }

        DbType IMySqlValue.DbType
        {
            get
            {
                if (TreatAsBoolean)
                    return DbType.Boolean;
                return DbType.SByte;
            }
        }

        object IMySqlValue.Value
        {
            get
            {
                if (TreatAsBoolean)
                    return Convert.ToBoolean(mValue);
                return mValue;
            }
        }

        Type IMySqlValue.SystemType
        {
            get
            {
                if (TreatAsBoolean)
                    return typeof(Boolean);
                return typeof(sbyte);
            }
        }

        string IMySqlValue.MySqlTypeName
        {
            get { return "TINYINT"; }
        }

        void IMySqlValue.WriteValue(MySqlStream stream, bool binary, object val, int length)
        {
            sbyte v = ((IConvertible)val).ToSByte(null);
            if (binary)
                stream.WriteByte((byte)v);
            else
                stream.WriteStringNoNull(v.ToString());
        }

        IMySqlValue IMySqlValue.ReadValue(MySqlStream stream, long length, bool nullVal)
        {
            if (nullVal)
                return new MySqlByte(true);

            if (length == -1)
                return new MySqlByte((sbyte)stream.ReadByte());
            else
            {
                string s = stream.ReadString(length);
                MySqlByte b = new MySqlByte(SByte.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture));
                b.TreatAsBoolean = TreatAsBoolean;
                return b;
            }
        }

        void IMySqlValue.SkipValue(MySqlStream stream)
        {
            stream.ReadByte();
        }

        #endregion
    }
}