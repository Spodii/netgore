using System;
using System.Data;
using System.Linq;
using DemoGame;
using NetGore;
using NetGore.AI;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class CharacterTemplateTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class CharacterTemplateTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this ICharacterTemplateTable source, DbParameterValues paramValues)
        {
            paramValues["@ai_id"] = (ushort?)source.AIID;
            paramValues["@alliance_id"] = (Byte)source.AllianceID;
            paramValues["@body_id"] = (UInt16)source.BodyID;
            paramValues["@exp"] = source.Exp;
            paramValues["@give_cash"] = source.GiveCash;
            paramValues["@give_exp"] = source.GiveExp;
            paramValues["@id"] = (UInt16)source.ID;
            paramValues["@level"] = source.Level;
            paramValues["@name"] = source.Name;
            paramValues["@respawn"] = source.Respawn;
            paramValues["@shop_id"] = (ushort?)source.ShopID;
            paramValues["@stat_agi"] = (Int16)source.GetStat(StatType.Agi);
            paramValues["@stat_defence"] = (Int16)source.GetStat(StatType.Defence);
            paramValues["@stat_int"] = (Int16)source.GetStat(StatType.Int);
            paramValues["@stat_maxhit"] = (Int16)source.GetStat(StatType.MaxHit);
            paramValues["@stat_maxhp"] = (Int16)source.GetStat(StatType.MaxHP);
            paramValues["@stat_maxmp"] = (Int16)source.GetStat(StatType.MaxMP);
            paramValues["@stat_minhit"] = (Int16)source.GetStat(StatType.MinHit);
            paramValues["@stat_str"] = (Int16)source.GetStat(StatType.Str);
            paramValues["@statpoints"] = source.StatPoints;
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this CharacterTemplateTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("ai_id");
            source.AIID = (AIID?)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("alliance_id");
            source.AllianceID = (AllianceID)dataReader.GetByte(i);

            i = dataReader.GetOrdinal("body_id");
            source.BodyID = (BodyIndex)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("exp");
            source.Exp = dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("give_cash");
            source.GiveCash = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("give_exp");
            source.GiveExp = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("id");
            source.ID = (CharacterTemplateID)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("level");
            source.Level = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("name");
            source.Name = dataReader.GetString(i);

            i = dataReader.GetOrdinal("respawn");
            source.Respawn = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("shop_id");
            source.ShopID = (ShopID?)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("stat_agi");
            source.SetStat(StatType.Agi, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_defence");
            source.SetStat(StatType.Defence, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_int");
            source.SetStat(StatType.Int, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_maxhit");
            source.SetStat(StatType.MaxHit, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_maxhp");
            source.SetStat(StatType.MaxHP, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_maxmp");
            source.SetStat(StatType.MaxMP, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_minhit");
            source.SetStat(StatType.MinHit, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("stat_str");
            source.SetStat(StatType.Str, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("statpoints");
            source.StatPoints = dataReader.GetInt32(i);
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
        public static void TryCopyValues(this ICharacterTemplateTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@ai_id":
                        paramValues[i] = (ushort?)source.AIID;
                        break;

                    case "@alliance_id":
                        paramValues[i] = (Byte)source.AllianceID;
                        break;

                    case "@body_id":
                        paramValues[i] = (UInt16)source.BodyID;
                        break;

                    case "@exp":
                        paramValues[i] = source.Exp;
                        break;

                    case "@give_cash":
                        paramValues[i] = source.GiveCash;
                        break;

                    case "@give_exp":
                        paramValues[i] = source.GiveExp;
                        break;

                    case "@id":
                        paramValues[i] = (UInt16)source.ID;
                        break;

                    case "@level":
                        paramValues[i] = source.Level;
                        break;

                    case "@name":
                        paramValues[i] = source.Name;
                        break;

                    case "@respawn":
                        paramValues[i] = source.Respawn;
                        break;

                    case "@shop_id":
                        paramValues[i] = (ushort?)source.ShopID;
                        break;

                    case "@stat_agi":
                        paramValues[i] = (Int16)source.GetStat(StatType.Agi);
                        break;

                    case "@stat_defence":
                        paramValues[i] = (Int16)source.GetStat(StatType.Defence);
                        break;

                    case "@stat_int":
                        paramValues[i] = (Int16)source.GetStat(StatType.Int);
                        break;

                    case "@stat_maxhit":
                        paramValues[i] = (Int16)source.GetStat(StatType.MaxHit);
                        break;

                    case "@stat_maxhp":
                        paramValues[i] = (Int16)source.GetStat(StatType.MaxHP);
                        break;

                    case "@stat_maxmp":
                        paramValues[i] = (Int16)source.GetStat(StatType.MaxMP);
                        break;

                    case "@stat_minhit":
                        paramValues[i] = (Int16)source.GetStat(StatType.MinHit);
                        break;

                    case "@stat_str":
                        paramValues[i] = (Int16)source.GetStat(StatType.Str);
                        break;

                    case "@statpoints":
                        paramValues[i] = source.StatPoints;
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
        public static void TryReadValues(this CharacterTemplateTable source, IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "ai_id":
                        source.AIID = (AIID?)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "alliance_id":
                        source.AllianceID = (AllianceID)dataReader.GetByte(i);
                        break;

                    case "body_id":
                        source.BodyID = (BodyIndex)dataReader.GetUInt16(i);
                        break;

                    case "exp":
                        source.Exp = dataReader.GetInt32(i);
                        break;

                    case "give_cash":
                        source.GiveCash = dataReader.GetUInt16(i);
                        break;

                    case "give_exp":
                        source.GiveExp = dataReader.GetUInt16(i);
                        break;

                    case "id":
                        source.ID = (CharacterTemplateID)dataReader.GetUInt16(i);
                        break;

                    case "level":
                        source.Level = dataReader.GetByte(i);
                        break;

                    case "name":
                        source.Name = dataReader.GetString(i);
                        break;

                    case "respawn":
                        source.Respawn = dataReader.GetUInt16(i);
                        break;

                    case "shop_id":
                        source.ShopID = (ShopID?)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "stat_agi":
                        source.SetStat(StatType.Agi, dataReader.GetInt16(i));
                        break;

                    case "stat_defence":
                        source.SetStat(StatType.Defence, dataReader.GetInt16(i));
                        break;

                    case "stat_int":
                        source.SetStat(StatType.Int, dataReader.GetInt16(i));
                        break;

                    case "stat_maxhit":
                        source.SetStat(StatType.MaxHit, dataReader.GetInt16(i));
                        break;

                    case "stat_maxhp":
                        source.SetStat(StatType.MaxHP, dataReader.GetInt16(i));
                        break;

                    case "stat_maxmp":
                        source.SetStat(StatType.MaxMP, dataReader.GetInt16(i));
                        break;

                    case "stat_minhit":
                        source.SetStat(StatType.MinHit, dataReader.GetInt16(i));
                        break;

                    case "stat_str":
                        source.SetStat(StatType.Str, dataReader.GetInt16(i));
                        break;

                    case "statpoints":
                        source.StatPoints = dataReader.GetInt32(i);
                        break;
                }
            }
        }
    }
}