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

This file was generated on (UTC): 5/21/2010 1:39:24 AM
********************************************************************/

using System;
using System.Data;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class WorldStatsNpcKillUserTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class WorldStatsNpcKillUserTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this IWorldStatsNpcKillUserTable source, DbParameterValues paramValues)
        {
            paramValues["@map_id"] = (ushort?)source.MapID;
            paramValues["@npc_template_id"] = (ushort?)source.NpcTemplateId;
            paramValues["@npc_x"] = source.NpcX;
            paramValues["@npc_y"] = source.NpcY;
            paramValues["@user_id"] = (Int32)source.UserId;
            paramValues["@user_level"] = source.UserLevel;
            paramValues["@user_x"] = source.UserX;
            paramValues["@user_y"] = source.UserY;
            paramValues["@when"] = source.When;
        }

        /// <summary>
        /// Checks if this <see cref="IWorldStatsNpcKillUserTable"/> contains the same values as another <see cref="IWorldStatsNpcKillUserTable"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IWorldStatsNpcKillUserTable"/>.</param>
        /// <param name="otherItem">The <see cref="IWorldStatsNpcKillUserTable"/> to compare the values to.</param>
        /// <returns>
        /// True if this <see cref="IWorldStatsNpcKillUserTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
        /// </returns>
        public static Boolean HasSameValues(this IWorldStatsNpcKillUserTable source, IWorldStatsNpcKillUserTable otherItem)
        {
            return Equals(source.MapID, otherItem.MapID) && Equals(source.NpcTemplateId, otherItem.NpcTemplateId) &&
                   Equals(source.NpcX, otherItem.NpcX) && Equals(source.NpcY, otherItem.NpcY) &&
                   Equals(source.UserId, otherItem.UserId) && Equals(source.UserLevel, otherItem.UserLevel) &&
                   Equals(source.UserX, otherItem.UserX) && Equals(source.UserY, otherItem.UserY) &&
                   Equals(source.When, otherItem.When);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this WorldStatsNpcKillUserTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("map_id");

            source.MapID = (Nullable<MapID>)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("npc_template_id");

            source.NpcTemplateId =
                (Nullable<CharacterTemplateID>)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("npc_x");

            source.NpcX = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("npc_y");

            source.NpcY = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("user_id");

            source.UserId = (CharacterID)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("user_level");

            source.UserLevel = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("user_x");

            source.UserX = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("user_y");

            source.UserY = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("when");

            source.When = dataReader.GetDateTime(i);
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
        public static void TryCopyValues(this IWorldStatsNpcKillUserTable source, DbParameterValues paramValues)
        {
            for (var i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@map_id":
                        paramValues[i] = (ushort?)source.MapID;
                        break;

                    case "@npc_template_id":
                        paramValues[i] = (ushort?)source.NpcTemplateId;
                        break;

                    case "@npc_x":
                        paramValues[i] = source.NpcX;
                        break;

                    case "@npc_y":
                        paramValues[i] = source.NpcY;
                        break;

                    case "@user_id":
                        paramValues[i] = (Int32)source.UserId;
                        break;

                    case "@user_level":
                        paramValues[i] = source.UserLevel;
                        break;

                    case "@user_x":
                        paramValues[i] = source.UserX;
                        break;

                    case "@user_y":
                        paramValues[i] = source.UserY;
                        break;

                    case "@when":
                        paramValues[i] = source.When;
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
        public static void TryReadValues(this WorldStatsNpcKillUserTable source, IDataReader dataReader)
        {
            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "map_id":
                        source.MapID = (Nullable<MapID>)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "npc_template_id":
                        source.NpcTemplateId =
                            (Nullable<CharacterTemplateID>)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "npc_x":
                        source.NpcX = dataReader.GetUInt16(i);
                        break;

                    case "npc_y":
                        source.NpcY = dataReader.GetUInt16(i);
                        break;

                    case "user_id":
                        source.UserId = (CharacterID)dataReader.GetInt32(i);
                        break;

                    case "user_level":
                        source.UserLevel = dataReader.GetByte(i);
                        break;

                    case "user_x":
                        source.UserX = dataReader.GetUInt16(i);
                        break;

                    case "user_y":
                        source.UserY = dataReader.GetUInt16(i);
                        break;

                    case "when":
                        source.When = dataReader.GetDateTime(i);
                        break;
                }
            }
        }
    }
}