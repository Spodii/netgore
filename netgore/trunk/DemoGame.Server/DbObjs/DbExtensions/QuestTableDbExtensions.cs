using System;
using System.Data;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Db;
using NetGore.Features.Quests;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class QuestTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class QuestTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this IQuestTable source, DbParameterValues paramValues)
        {
            paramValues["@id"] = (UInt16)source.ID;
            paramValues["@repeatable"] = source.Repeatable;
            paramValues["@reward_cash"] = source.RewardCash;
            paramValues["@reward_exp"] = source.RewardExp;
        }

        /// <summary>
        /// Checks if this <see cref="IQuestTable"/> contains the same values as an<paramref name="other"/> <see cref="IQuestTable"/>.
        /// </summary>
        /// <param name="other">The <see cref="IQuestTable"/> to compare the values to.</param>
        /// <returns>
        /// True if this <see cref="IQuestTable"/> contains the same values as the <paramref name="<paramref name="other"/>"/>; <paramref name="other"/>wise false.
        /// </returns>
        public static Boolean HasSameValues(this IQuestTable source, IQuestTable other)
        {
            return Equals(source.ID, other.ID) && Equals(source.Repeatable, other.Repeatable) &&
                   Equals(source.RewardCash, other.RewardCash) && Equals(source.RewardExp, other.RewardExp);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this QuestTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("id");

            source.ID = (QuestID)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("repeatable");

            source.Repeatable = dataReader.GetBoolean(i);

            i = dataReader.GetOrdinal("reward_cash");

            source.RewardCash = dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("reward_exp");

            source.RewardExp = dataReader.GetInt32(i);
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
        public static void TryCopyValues(this IQuestTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@id":
                        paramValues[i] = (UInt16)source.ID;
                        break;

                    case "@repeatable":
                        paramValues[i] = source.Repeatable;
                        break;

                    case "@reward_cash":
                        paramValues[i] = source.RewardCash;
                        break;

                    case "@reward_exp":
                        paramValues[i] = source.RewardExp;
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
        public static void TryReadValues(this QuestTable source, IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "id":
                        source.ID = (QuestID)dataReader.GetUInt16(i);
                        break;

                    case "repeatable":
                        source.Repeatable = dataReader.GetBoolean(i);
                        break;

                    case "reward_cash":
                        source.RewardCash = dataReader.GetInt32(i);
                        break;

                    case "reward_exp":
                        source.RewardExp = dataReader.GetInt32(i);
                        break;
                }
            }
        }
    }
}