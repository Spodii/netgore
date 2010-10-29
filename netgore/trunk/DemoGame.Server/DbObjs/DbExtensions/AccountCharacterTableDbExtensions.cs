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
    /// Contains extension methods for class AccountCharacterTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class AccountCharacterTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this IAccountCharacterTable source, DbParameterValues paramValues)
        {
            paramValues["account_id"] = (Int32)source.AccountID;
            paramValues["character_id"] = (Int32)source.CharacterID;
            paramValues["time_deleted"] = source.TimeDeleted;
        }

        /// <summary>
        /// Checks if this <see cref="IAccountCharacterTable"/> contains the same values as another <see cref="IAccountCharacterTable"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IAccountCharacterTable"/>.</param>
        /// <param name="otherItem">The <see cref="IAccountCharacterTable"/> to compare the values to.</param>
        /// <returns>
        /// True if this <see cref="IAccountCharacterTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
        /// </returns>
        public static Boolean HasSameValues(this IAccountCharacterTable source, IAccountCharacterTable otherItem)
        {
            return Equals(source.AccountID, otherItem.AccountID) && Equals(source.CharacterID, otherItem.CharacterID) &&
                   Equals(source.TimeDeleted, otherItem.TimeDeleted);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this AccountCharacterTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("account_id");

            source.AccountID = (AccountID)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("character_id");

            source.CharacterID = (CharacterID)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("time_deleted");

            source.TimeDeleted = (dataReader.IsDBNull(i) ? (DateTime?)null : dataReader.GetDateTime(i));
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
        public static void TryCopyValues(this IAccountCharacterTable source, DbParameterValues paramValues)
        {
            for (var i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "account_id":
                        paramValues[i] = (Int32)source.AccountID;
                        break;

                    case "character_id":
                        paramValues[i] = (Int32)source.CharacterID;
                        break;

                    case "time_deleted":
                        paramValues[i] = source.TimeDeleted;
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
        public static void TryReadValues(this AccountCharacterTable source, IDataReader dataReader)
        {
            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "account_id":
                        source.AccountID = (AccountID)dataReader.GetInt32(i);
                        break;

                    case "character_id":
                        source.CharacterID = (CharacterID)dataReader.GetInt32(i);
                        break;

                    case "time_deleted":
                        source.TimeDeleted = (dataReader.IsDBNull(i) ? (DateTime?)null : dataReader.GetDateTime(i));
                        break;
                }
            }
        }
    }
}