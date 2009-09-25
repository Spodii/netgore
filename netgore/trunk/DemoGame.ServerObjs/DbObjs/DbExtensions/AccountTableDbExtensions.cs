using System;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class AccountTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class AccountTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this IAccountTable source, DbParameterValues paramValues)
        {
            paramValues["@current_ip"] = source.CurrentIp;
            paramValues["@email"] = source.Email;
            paramValues["@id"] = (Int32)source.ID;
            paramValues["@name"] = source.Name;
            paramValues["@password"] = source.Password;
            paramValues["@time_created"] = source.TimeCreated;
            paramValues["@time_last_login"] = source.TimeLastLogin;
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this AccountTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("current_ip");
            source.CurrentIp = (dataReader.IsDBNull(i) ? (Nullable<UInt32>)null : dataReader.GetUInt32(i));

            i = dataReader.GetOrdinal("email");
            source.Email = dataReader.GetString(i);

            i = dataReader.GetOrdinal("id");
            source.ID = (AccountID)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("name");
            source.Name = dataReader.GetString(i);

            i = dataReader.GetOrdinal("password");
            source.Password = dataReader.GetString(i);

            i = dataReader.GetOrdinal("time_created");
            source.TimeCreated = dataReader.GetDateTime(i);

            i = dataReader.GetOrdinal("time_last_login");
            source.TimeLastLogin = dataReader.GetDateTime(i);
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
        public static void TryCopyValues(this IAccountTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@current_ip":
                        paramValues[i] = source.CurrentIp;
                        break;

                    case "@email":
                        paramValues[i] = source.Email;
                        break;

                    case "@id":
                        paramValues[i] = (Int32)source.ID;
                        break;

                    case "@name":
                        paramValues[i] = source.Name;
                        break;

                    case "@password":
                        paramValues[i] = source.Password;
                        break;

                    case "@time_created":
                        paramValues[i] = source.TimeCreated;
                        break;

                    case "@time_last_login":
                        paramValues[i] = source.TimeLastLogin;
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
        public static void TryReadValues(this AccountTable source, IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "current_ip":
                        source.CurrentIp = (dataReader.IsDBNull(i) ? (Nullable<UInt32>)null : dataReader.GetUInt32(i));
                        break;

                    case "email":
                        source.Email = dataReader.GetString(i);
                        break;

                    case "id":
                        source.ID = (AccountID)dataReader.GetInt32(i);
                        break;

                    case "name":
                        source.Name = dataReader.GetString(i);
                        break;

                    case "password":
                        source.Password = dataReader.GetString(i);
                        break;

                    case "time_created":
                        source.TimeCreated = dataReader.GetDateTime(i);
                        break;

                    case "time_last_login":
                        source.TimeLastLogin = dataReader.GetDateTime(i);
                        break;
                }
            }
        }
    }
}