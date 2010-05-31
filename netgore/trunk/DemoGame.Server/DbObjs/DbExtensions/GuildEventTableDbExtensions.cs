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

This file was generated on (UTC): 5/31/2010 6:31:05 PM
********************************************************************/

using System;
using System.Data;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Db;
using NetGore.Features.Guilds;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class GuildEventTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class GuildEventTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this IGuildEventTable source, DbParameterValues paramValues)
        {
            paramValues["@arg0"] = source.Arg0;
            paramValues["@arg1"] = source.Arg1;
            paramValues["@arg2"] = source.Arg2;
            paramValues["@character_id"] = (Int32)source.CharacterID;
            paramValues["@created"] = source.Created;
            paramValues["@event_id"] = source.EventID;
            paramValues["@guild_id"] = (UInt16)source.GuildID;
            paramValues["@id"] = source.ID;
            paramValues["@target_character_id"] = (int?)source.TargetCharacterID;
        }

        /// <summary>
        /// Checks if this <see cref="IGuildEventTable"/> contains the same values as another <see cref="IGuildEventTable"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IGuildEventTable"/>.</param>
        /// <param name="otherItem">The <see cref="IGuildEventTable"/> to compare the values to.</param>
        /// <returns>
        /// True if this <see cref="IGuildEventTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
        /// </returns>
        public static Boolean HasSameValues(this IGuildEventTable source, IGuildEventTable otherItem)
        {
            return Equals(source.Arg0, otherItem.Arg0) && Equals(source.Arg1, otherItem.Arg1) &&
                   Equals(source.Arg2, otherItem.Arg2) && Equals(source.CharacterID, otherItem.CharacterID) &&
                   Equals(source.Created, otherItem.Created) && Equals(source.EventID, otherItem.EventID) &&
                   Equals(source.GuildID, otherItem.GuildID) && Equals(source.ID, otherItem.ID) &&
                   Equals(source.TargetCharacterID, otherItem.TargetCharacterID);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this GuildEventTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("arg0");

            source.Arg0 = (dataReader.IsDBNull(i) ? null : dataReader.GetString(i));

            i = dataReader.GetOrdinal("arg1");

            source.Arg1 = (dataReader.IsDBNull(i) ? null : dataReader.GetString(i));

            i = dataReader.GetOrdinal("arg2");

            source.Arg2 = (dataReader.IsDBNull(i) ? null : dataReader.GetString(i));

            i = dataReader.GetOrdinal("character_id");

            source.CharacterID = (CharacterID)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("created");

            source.Created = dataReader.GetDateTime(i);

            i = dataReader.GetOrdinal("event_id");

            source.EventID = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("guild_id");

            source.GuildID = (GuildID)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("id");

            source.ID = dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("target_character_id");

            source.TargetCharacterID = (CharacterID?)(dataReader.IsDBNull(i) ? (int?)null : dataReader.GetInt32(i));
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
        public static void TryCopyValues(this IGuildEventTable source, DbParameterValues paramValues)
        {
            for (var i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@arg0":
                        paramValues[i] = source.Arg0;
                        break;

                    case "@arg1":
                        paramValues[i] = source.Arg1;
                        break;

                    case "@arg2":
                        paramValues[i] = source.Arg2;
                        break;

                    case "@character_id":
                        paramValues[i] = (Int32)source.CharacterID;
                        break;

                    case "@created":
                        paramValues[i] = source.Created;
                        break;

                    case "@event_id":
                        paramValues[i] = source.EventID;
                        break;

                    case "@guild_id":
                        paramValues[i] = (UInt16)source.GuildID;
                        break;

                    case "@id":
                        paramValues[i] = source.ID;
                        break;

                    case "@target_character_id":
                        paramValues[i] = (int?)source.TargetCharacterID;
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
        public static void TryReadValues(this GuildEventTable source, IDataReader dataReader)
        {
            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "arg0":
                        source.Arg0 = (dataReader.IsDBNull(i) ? null : dataReader.GetString(i));
                        break;

                    case "arg1":
                        source.Arg1 = (dataReader.IsDBNull(i) ? null : dataReader.GetString(i));
                        break;

                    case "arg2":
                        source.Arg2 = (dataReader.IsDBNull(i) ? null : dataReader.GetString(i));
                        break;

                    case "character_id":
                        source.CharacterID = (CharacterID)dataReader.GetInt32(i);
                        break;

                    case "created":
                        source.Created = dataReader.GetDateTime(i);
                        break;

                    case "event_id":
                        source.EventID = dataReader.GetByte(i);
                        break;

                    case "guild_id":
                        source.GuildID = (GuildID)dataReader.GetUInt16(i);
                        break;

                    case "id":
                        source.ID = dataReader.GetInt32(i);
                        break;

                    case "target_character_id":
                        source.TargetCharacterID = (CharacterID?)(dataReader.IsDBNull(i) ? (int?)null : dataReader.GetInt32(i));
                        break;
                }
            }
        }
    }
}