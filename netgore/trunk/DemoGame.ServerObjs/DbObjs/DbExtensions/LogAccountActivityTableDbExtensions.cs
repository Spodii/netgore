using System;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class LogAccountActivityTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class LogAccountActivityTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this ILogAccountActivityTable source, DbParameterValues paramValues)
        {
            paramValues["@account_id"] = source.AccountId;
            paramValues["@id"] = source.ID;
            paramValues["@ip"] = source.Ip;
            paramValues["@time_login"] = source.TimeLogin;
            paramValues["@time_logout"] = source.TimeLogout;
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this LogAccountActivityTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("account_id");
            source.AccountId = (Int32)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("id");
            source.ID = (Int32)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("ip");
            source.Ip = (UInt32)dataReader.GetUInt32(i);

            i = dataReader.GetOrdinal("time_login");
            source.TimeLogin = (DateTime)dataReader.GetDateTime(i);

            i = dataReader.GetOrdinal("time_logout");
            source.TimeLogout =
                (Nullable<DateTime>)(dataReader.IsDBNull(i) ? (Nullable<DateTime>)null : dataReader.GetDateTime(i));
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The key must already exist in the DbParameterValues
        /// for the value to be copied over. If any of the keys in the DbParameterValues do not
        /// match one of the column names, or if there is no field for a key, then it will be
        /// ignored. Because of this, it is important to be careful when using this method
        /// since columns or keys can be skipped without any indication.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void TryCopyValues(this ILogAccountActivityTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@account_id":
                        paramValues[i] = source.AccountId;
                        break;

                    case "@id":
                        paramValues[i] = source.ID;
                        break;

                    case "@ip":
                        paramValues[i] = source.Ip;
                        break;

                    case "@time_login":
                        paramValues[i] = source.TimeLogin;
                        break;

                    case "@time_logout":
                        paramValues[i] = source.TimeLogout;
                        break;
                }
            }
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. Unlike ReadValues(), this method not only doesn't require
        /// all values to be in the IDataReader, but also does not require the values in
        /// the IDataReader to be a defined field for the table this class represents.
        /// Because of this, you need to be careful when using this method because values
        /// can easily be skipped without any indication.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void TryReadValues(this LogAccountActivityTable source, IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "account_id":
                        source.AccountId = (Int32)dataReader.GetInt32(i);
                        break;

                    case "id":
                        source.ID = (Int32)dataReader.GetInt32(i);
                        break;

                    case "ip":
                        source.Ip = (UInt32)dataReader.GetUInt32(i);
                        break;

                    case "time_login":
                        source.TimeLogin = (DateTime)dataReader.GetDateTime(i);
                        break;

                    case "time_logout":
                        source.TimeLogout =
                            (Nullable<DateTime>)(dataReader.IsDBNull(i) ? (Nullable<DateTime>)null : dataReader.GetDateTime(i));
                        break;
                }
            }
        }
    }
}