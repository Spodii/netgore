/********************************************************************
                   DO NOT MANUALLY EDIT THIS FILE!

This file was automatically generated using the DbClassCreator
program. The only time you should ever alter this file is if you are
using an automated code formatter. The DbClassCreator will overwrite
this file every time it is run, so all manual changes will be lost.
If there is something in this file that you wish to change, you should
be able to do it through the DbClassCreator arguments.

Make sure that you re-run the DbClassCreator every time you alter your
game's database.

For more information on the DbClassCreator, please see:
    http://www.netgore.com/wiki/dbclasscreator.html
********************************************************************/

using System;
using System.Data;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class AccountBanTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class AccountBanTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this IAccountBanTable source, DbParameterValues paramValues)
        {
            paramValues["account_id"] = (Int32)source.AccountID;
            paramValues["end_time"] = source.EndTime;
            paramValues["expired"] = source.Expired;
            paramValues["id"] = source.ID;
            paramValues["issued_by"] = source.IssuedBy;
            paramValues["reason"] = source.Reason;
            paramValues["start_time"] = source.StartTime;
        }

        /// <summary>
        /// Checks if this <see cref="IAccountBanTable"/> contains the same values as another <see cref="IAccountBanTable"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IAccountBanTable"/>.</param>
        /// <param name="otherItem">The <see cref="IAccountBanTable"/> to compare the values to.</param>
        /// <returns>
        /// True if this <see cref="IAccountBanTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
        /// </returns>
        public static Boolean HasSameValues(this IAccountBanTable source, IAccountBanTable otherItem)
        {
            return Equals(source.AccountID, otherItem.AccountID) && Equals(source.EndTime, otherItem.EndTime) &&
                   Equals(source.Expired, otherItem.Expired) && Equals(source.ID, otherItem.ID) &&
                   Equals(source.IssuedBy, otherItem.IssuedBy) && Equals(source.Reason, otherItem.Reason) &&
                   Equals(source.StartTime, otherItem.StartTime);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this AccountBanTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("account_id");

            source.AccountID = (AccountID)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("end_time");

            source.EndTime = dataReader.GetDateTime(i);

            i = dataReader.GetOrdinal("expired");

            source.Expired = dataReader.GetBoolean(i);

            i = dataReader.GetOrdinal("id");

            source.ID = dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("issued_by");

            source.IssuedBy = (dataReader.IsDBNull(i) ? null : dataReader.GetString(i));

            i = dataReader.GetOrdinal("reason");

            source.Reason = dataReader.GetString(i);

            i = dataReader.GetOrdinal("start_time");

            source.StartTime = dataReader.GetDateTime(i);
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
        public static void TryCopyValues(this IAccountBanTable source, DbParameterValues paramValues)
        {
            for (var i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "account_id":
                        paramValues[i] = (Int32)source.AccountID;
                        break;

                    case "end_time":
                        paramValues[i] = source.EndTime;
                        break;

                    case "expired":
                        paramValues[i] = source.Expired;
                        break;

                    case "id":
                        paramValues[i] = source.ID;
                        break;

                    case "issued_by":
                        paramValues[i] = source.IssuedBy;
                        break;

                    case "reason":
                        paramValues[i] = source.Reason;
                        break;

                    case "start_time":
                        paramValues[i] = source.StartTime;
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
        public static void TryReadValues(this AccountBanTable source, IDataReader dataReader)
        {
            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "account_id":
                        source.AccountID = (AccountID)dataReader.GetInt32(i);
                        break;

                    case "end_time":
                        source.EndTime = dataReader.GetDateTime(i);
                        break;

                    case "expired":
                        source.Expired = dataReader.GetBoolean(i);
                        break;

                    case "id":
                        source.ID = dataReader.GetInt32(i);
                        break;

                    case "issued_by":
                        source.IssuedBy = (dataReader.IsDBNull(i) ? null : dataReader.GetString(i));
                        break;

                    case "reason":
                        source.Reason = dataReader.GetString(i);
                        break;

                    case "start_time":
                        source.StartTime = dataReader.GetDateTime(i);
                        break;
                }
            }
        }
    }
}