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

This file was generated on (UTC): 3/22/2010 2:25:30 AM
********************************************************************/

using System;
using System.Data;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Db;
using NetGore.Features.Quests;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class CharacterQuestStatusTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class CharacterQuestStatusTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this ICharacterQuestStatusTable source, DbParameterValues paramValues)
        {
            paramValues["@character_id"] = (Int32)source.CharacterID;
            paramValues["@completed_on"] = source.CompletedOn;
            paramValues["@quest_id"] = (UInt16)source.QuestID;
            paramValues["@started_on"] = source.StartedOn;
        }

        /// <summary>
        /// Checks if this <see cref="ICharacterQuestStatusTable"/> contains the same values as an<paramref name="other"/> <see cref="ICharacterQuestStatusTable"/>.
        /// </summary>
        /// <param name="other">The <see cref="ICharacterQuestStatusTable"/> to compare the values to.</param>
        /// <returns>
        /// True if this <see cref="ICharacterQuestStatusTable"/> contains the same values as the <paramref name="<paramref name="other"/>"/>; <paramref name="other"/>wise false.
        /// </returns>
        public static Boolean HasSameValues(this ICharacterQuestStatusTable source, ICharacterQuestStatusTable other)
        {
            return Equals(source.CharacterID, other.CharacterID) && Equals(source.CompletedOn, other.CompletedOn) &&
                   Equals(source.QuestID, other.QuestID) && Equals(source.StartedOn, other.StartedOn);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this CharacterQuestStatusTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("character_id");

            source.CharacterID = (CharacterID)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("completed_on");

            source.CompletedOn = (dataReader.IsDBNull(i) ? (DateTime?)null : dataReader.GetDateTime(i));

            i = dataReader.GetOrdinal("quest_id");

            source.QuestID = (QuestID)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("started_on");

            source.StartedOn = dataReader.GetDateTime(i);
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
        public static void TryCopyValues(this ICharacterQuestStatusTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@character_id":
                        paramValues[i] = (Int32)source.CharacterID;
                        break;

                    case "@completed_on":
                        paramValues[i] = source.CompletedOn;
                        break;

                    case "@quest_id":
                        paramValues[i] = (UInt16)source.QuestID;
                        break;

                    case "@started_on":
                        paramValues[i] = source.StartedOn;
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
        public static void TryReadValues(this CharacterQuestStatusTable source, IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "character_id":
                        source.CharacterID = (CharacterID)dataReader.GetInt32(i);
                        break;

                    case "completed_on":
                        source.CompletedOn = (dataReader.IsDBNull(i) ? (DateTime?)null : dataReader.GetDateTime(i));
                        break;

                    case "quest_id":
                        source.QuestID = (QuestID)dataReader.GetUInt16(i);
                        break;

                    case "started_on":
                        source.StartedOn = dataReader.GetDateTime(i);
                        break;
                }
            }
        }
    }
}