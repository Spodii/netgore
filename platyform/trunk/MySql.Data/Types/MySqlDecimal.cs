using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    struct MySqlDecimal : IMySqlValue
    {
        readonly bool isNull;
        readonly Decimal mValue;
        byte precision;
        byte scale;

        public byte Precision
        {
            get { return precision; }
            set { precision = value; }
        }

        public byte Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public decimal Value
        {
            get { return mValue; }
        }

        public MySqlDecimal(bool isNull)
        {
            this.isNull = isNull;
            mValue = 0;
            precision = scale = 0;
        }

        public MySqlDecimal(decimal val)
        {
            isNull = false;
            precision = scale = 0;
            mValue = val;
        }

        internal static void SetDSInfo(DataTable dsTable)
        {
            // we use name indexing because this method will only be called
            // when GetSchema is called for the DataSourceInformation 
            // collection and then it wil be cached.
            DataRow row = dsTable.NewRow();
            row["TypeName"] = "DECIMAL";
            row["ProviderDbType"] = MySqlDbType.NewDecimal;
            row["ColumnSize"] = 0;
            row["CreateFormat"] = "DECIMAL({0},{1})";
            row["CreateParameters"] = "precision,scale";
            row["DataType"] = "System.Decimal";
            row["IsAutoincrementable"] = false;
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
            get { return MySqlDbType.Decimal; }
        }

        DbType IMySqlValue.DbType
        {
            get { return DbType.Decimal; }
        }

        object IMySqlValue.Value
        {
            get { return mValue; }
        }

        Type IMySqlValue.SystemType
        {
            get { return typeof(decimal); }
        }

        string IMySqlValue.MySqlTypeName
        {
            get { return "DECIMAL"; }
        }

        void IMySqlValue.WriteValue(MySqlStream stream, bool binary, object val, int length)
        {
            decimal v = Convert.ToDecimal(val);
            string valStr = v.ToString(CultureInfo.InvariantCulture);
            if (binary)
                stream.WriteLenString(valStr);
            else
                stream.WriteStringNoNull(valStr);
        }

        IMySqlValue IMySqlValue.ReadValue(MySqlStream stream, long length, bool nullVal)
        {
            if (nullVal)
                return new MySqlDecimal(true);

            if (length == -1)
            {
                string s = stream.ReadLenString();
                return new MySqlDecimal(Decimal.Parse(s, CultureInfo.InvariantCulture));
            }
            else
            {
                string s = stream.ReadString(length);
                return new MySqlDecimal(Decimal.Parse(s, CultureInfo.InvariantCulture));
            }
        }

        void IMySqlValue.SkipValue(MySqlStream stream)
        {
            long len = stream.ReadFieldLength();
            stream.SkipBytes((int)len);
        }

        #endregion
    }
}