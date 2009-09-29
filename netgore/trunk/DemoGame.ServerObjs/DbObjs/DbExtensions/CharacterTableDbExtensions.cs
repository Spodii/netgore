using System;
using System.Data;
using System.Linq;
using NetGore;
using NetGore.Db;
using NetGore.RPGComponents;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class CharacterTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class CharacterTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this ICharacterTable source, DbParameterValues paramValues)
        {
            paramValues["@acc"] = (Byte)source.GetStat(StatType.Acc);
            paramValues["@account_id"] = (int?)source.AccountID;
            paramValues["@agi"] = (Byte)source.GetStat(StatType.Agi);
            paramValues["@armor"] = (Byte)source.GetStat(StatType.Armor);
            paramValues["@body_id"] = (UInt16)source.BodyID;
            paramValues["@bra"] = (Byte)source.GetStat(StatType.Bra);
            paramValues["@cash"] = source.Cash;
            paramValues["@character_template_id"] = (ushort?)source.CharacterTemplateID;
            paramValues["@chat_dialog"] = source.ChatDialog;
            paramValues["@defence"] = (Byte)source.GetStat(StatType.Defence);
            paramValues["@dex"] = (Byte)source.GetStat(StatType.Dex);
            paramValues["@evade"] = (Byte)source.GetStat(StatType.Evade);
            paramValues["@exp"] = source.Exp;
            paramValues["@hp"] = (Int16)source.HP;
            paramValues["@id"] = (Int32)source.ID;
            paramValues["@imm"] = (Byte)source.GetStat(StatType.Imm);
            paramValues["@int"] = (Byte)source.GetStat(StatType.Int);
            paramValues["@level"] = source.Level;
            paramValues["@map_id"] = (UInt16)source.MapID;
            paramValues["@maxhit"] = (Byte)source.GetStat(StatType.MaxHit);
            paramValues["@maxhp"] = (Int16)source.GetStat(StatType.MaxHP);
            paramValues["@maxmp"] = (Int16)source.GetStat(StatType.MaxMP);
            paramValues["@minhit"] = (Byte)source.GetStat(StatType.MinHit);
            paramValues["@mp"] = (Int16)source.MP;
            paramValues["@name"] = source.Name;
            paramValues["@perc"] = (Byte)source.GetStat(StatType.Perc);
            paramValues["@recov"] = (Byte)source.GetStat(StatType.Recov);
            paramValues["@regen"] = (Byte)source.GetStat(StatType.Regen);
            paramValues["@respawn_map"] = (ushort?)source.RespawnMap;
            paramValues["@respawn_x"] = source.RespawnX;
            paramValues["@respawn_y"] = source.RespawnY;
            paramValues["@shop_id"] = (ushort?)source.ShopID;
            paramValues["@statpoints"] = source.StatPoints;
            paramValues["@str"] = (Byte)source.GetStat(StatType.Str);
            paramValues["@tact"] = (Byte)source.GetStat(StatType.Tact);
            paramValues["@ws"] = (Byte)source.GetStat(StatType.WS);
            paramValues["@x"] = source.X;
            paramValues["@y"] = source.Y;
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this CharacterTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("acc");
            source.SetStat(StatType.Acc, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("account_id");
            source.AccountID = (AccountID?)(dataReader.IsDBNull(i) ? (int?)null : dataReader.GetInt32(i));

            i = dataReader.GetOrdinal("agi");
            source.SetStat(StatType.Agi, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("armor");
            source.SetStat(StatType.Armor, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("body_id");
            source.BodyID = (BodyIndex)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("bra");
            source.SetStat(StatType.Bra, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("cash");
            source.Cash = dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("character_template_id");
            source.CharacterTemplateID = (CharacterTemplateID?)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("chat_dialog");
            source.ChatDialog = (dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("defence");
            source.SetStat(StatType.Defence, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("dex");
            source.SetStat(StatType.Dex, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("evade");
            source.SetStat(StatType.Evade, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("exp");
            source.Exp = dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("hp");
            source.HP = dataReader.GetInt16(i);

            i = dataReader.GetOrdinal("id");
            source.ID = (CharacterID)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("imm");
            source.SetStat(StatType.Imm, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("int");
            source.SetStat(StatType.Int, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("level");
            source.Level = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("map_id");
            source.MapID = (MapIndex)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("maxhit");
            source.SetStat(StatType.MaxHit, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("maxhp");
            source.SetStat(StatType.MaxHP, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("maxmp");
            source.SetStat(StatType.MaxMP, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("minhit");
            source.SetStat(StatType.MinHit, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("mp");
            source.MP = dataReader.GetInt16(i);

            i = dataReader.GetOrdinal("name");
            source.Name = dataReader.GetString(i);

            i = dataReader.GetOrdinal("perc");
            source.SetStat(StatType.Perc, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("recov");
            source.SetStat(StatType.Recov, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("regen");
            source.SetStat(StatType.Regen, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("respawn_map");
            source.RespawnMap = (MapIndex?)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("respawn_x");
            source.RespawnX = dataReader.GetFloat(i);

            i = dataReader.GetOrdinal("respawn_y");
            source.RespawnY = dataReader.GetFloat(i);

            i = dataReader.GetOrdinal("shop_id");
            source.ShopID = (ShopID?)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("statpoints");
            source.StatPoints = dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("str");
            source.SetStat(StatType.Str, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("tact");
            source.SetStat(StatType.Tact, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("ws");
            source.SetStat(StatType.WS, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("x");
            source.X = dataReader.GetFloat(i);

            i = dataReader.GetOrdinal("y");
            source.Y = dataReader.GetFloat(i);
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
        public static void TryCopyValues(this ICharacterTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@acc":
                        paramValues[i] = (Byte)source.GetStat(StatType.Acc);
                        break;

                    case "@account_id":
                        paramValues[i] = (int?)source.AccountID;
                        break;

                    case "@agi":
                        paramValues[i] = (Byte)source.GetStat(StatType.Agi);
                        break;

                    case "@armor":
                        paramValues[i] = (Byte)source.GetStat(StatType.Armor);
                        break;

                    case "@body_id":
                        paramValues[i] = (UInt16)source.BodyID;
                        break;

                    case "@bra":
                        paramValues[i] = (Byte)source.GetStat(StatType.Bra);
                        break;

                    case "@cash":
                        paramValues[i] = source.Cash;
                        break;

                    case "@character_template_id":
                        paramValues[i] = (ushort?)source.CharacterTemplateID;
                        break;

                    case "@chat_dialog":
                        paramValues[i] = source.ChatDialog;
                        break;

                    case "@defence":
                        paramValues[i] = (Byte)source.GetStat(StatType.Defence);
                        break;

                    case "@dex":
                        paramValues[i] = (Byte)source.GetStat(StatType.Dex);
                        break;

                    case "@evade":
                        paramValues[i] = (Byte)source.GetStat(StatType.Evade);
                        break;

                    case "@exp":
                        paramValues[i] = source.Exp;
                        break;

                    case "@hp":
                        paramValues[i] = (Int16)source.HP;
                        break;

                    case "@id":
                        paramValues[i] = (Int32)source.ID;
                        break;

                    case "@imm":
                        paramValues[i] = (Byte)source.GetStat(StatType.Imm);
                        break;

                    case "@int":
                        paramValues[i] = (Byte)source.GetStat(StatType.Int);
                        break;

                    case "@level":
                        paramValues[i] = source.Level;
                        break;

                    case "@map_id":
                        paramValues[i] = (UInt16)source.MapID;
                        break;

                    case "@maxhit":
                        paramValues[i] = (Byte)source.GetStat(StatType.MaxHit);
                        break;

                    case "@maxhp":
                        paramValues[i] = (Int16)source.GetStat(StatType.MaxHP);
                        break;

                    case "@maxmp":
                        paramValues[i] = (Int16)source.GetStat(StatType.MaxMP);
                        break;

                    case "@minhit":
                        paramValues[i] = (Byte)source.GetStat(StatType.MinHit);
                        break;

                    case "@mp":
                        paramValues[i] = (Int16)source.MP;
                        break;

                    case "@name":
                        paramValues[i] = source.Name;
                        break;

                    case "@perc":
                        paramValues[i] = (Byte)source.GetStat(StatType.Perc);
                        break;

                    case "@recov":
                        paramValues[i] = (Byte)source.GetStat(StatType.Recov);
                        break;

                    case "@regen":
                        paramValues[i] = (Byte)source.GetStat(StatType.Regen);
                        break;

                    case "@respawn_map":
                        paramValues[i] = (ushort?)source.RespawnMap;
                        break;

                    case "@respawn_x":
                        paramValues[i] = source.RespawnX;
                        break;

                    case "@respawn_y":
                        paramValues[i] = source.RespawnY;
                        break;

                    case "@shop_id":
                        paramValues[i] = (ushort?)source.ShopID;
                        break;

                    case "@statpoints":
                        paramValues[i] = source.StatPoints;
                        break;

                    case "@str":
                        paramValues[i] = (Byte)source.GetStat(StatType.Str);
                        break;

                    case "@tact":
                        paramValues[i] = (Byte)source.GetStat(StatType.Tact);
                        break;

                    case "@ws":
                        paramValues[i] = (Byte)source.GetStat(StatType.WS);
                        break;

                    case "@x":
                        paramValues[i] = source.X;
                        break;

                    case "@y":
                        paramValues[i] = source.Y;
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
        public static void TryReadValues(this CharacterTable source, IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "acc":
                        source.SetStat(StatType.Acc, dataReader.GetByte(i));
                        break;

                    case "account_id":
                        source.AccountID = (AccountID?)(dataReader.IsDBNull(i) ? (int?)null : dataReader.GetInt32(i));
                        break;

                    case "agi":
                        source.SetStat(StatType.Agi, dataReader.GetByte(i));
                        break;

                    case "armor":
                        source.SetStat(StatType.Armor, dataReader.GetByte(i));
                        break;

                    case "body_id":
                        source.BodyID = (BodyIndex)dataReader.GetUInt16(i);
                        break;

                    case "bra":
                        source.SetStat(StatType.Bra, dataReader.GetByte(i));
                        break;

                    case "cash":
                        source.Cash = dataReader.GetInt32(i);
                        break;

                    case "character_template_id":
                        source.CharacterTemplateID =
                            (CharacterTemplateID?)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "chat_dialog":
                        source.ChatDialog = (dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "defence":
                        source.SetStat(StatType.Defence, dataReader.GetByte(i));
                        break;

                    case "dex":
                        source.SetStat(StatType.Dex, dataReader.GetByte(i));
                        break;

                    case "evade":
                        source.SetStat(StatType.Evade, dataReader.GetByte(i));
                        break;

                    case "exp":
                        source.Exp = dataReader.GetInt32(i);
                        break;

                    case "hp":
                        source.HP = dataReader.GetInt16(i);
                        break;

                    case "id":
                        source.ID = (CharacterID)dataReader.GetInt32(i);
                        break;

                    case "imm":
                        source.SetStat(StatType.Imm, dataReader.GetByte(i));
                        break;

                    case "int":
                        source.SetStat(StatType.Int, dataReader.GetByte(i));
                        break;

                    case "level":
                        source.Level = dataReader.GetByte(i);
                        break;

                    case "map_id":
                        source.MapID = (MapIndex)dataReader.GetUInt16(i);
                        break;

                    case "maxhit":
                        source.SetStat(StatType.MaxHit, dataReader.GetByte(i));
                        break;

                    case "maxhp":
                        source.SetStat(StatType.MaxHP, dataReader.GetInt16(i));
                        break;

                    case "maxmp":
                        source.SetStat(StatType.MaxMP, dataReader.GetInt16(i));
                        break;

                    case "minhit":
                        source.SetStat(StatType.MinHit, dataReader.GetByte(i));
                        break;

                    case "mp":
                        source.MP = dataReader.GetInt16(i);
                        break;

                    case "name":
                        source.Name = dataReader.GetString(i);
                        break;

                    case "perc":
                        source.SetStat(StatType.Perc, dataReader.GetByte(i));
                        break;

                    case "recov":
                        source.SetStat(StatType.Recov, dataReader.GetByte(i));
                        break;

                    case "regen":
                        source.SetStat(StatType.Regen, dataReader.GetByte(i));
                        break;

                    case "respawn_map":
                        source.RespawnMap = (MapIndex?)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "respawn_x":
                        source.RespawnX = dataReader.GetFloat(i);
                        break;

                    case "respawn_y":
                        source.RespawnY = dataReader.GetFloat(i);
                        break;

                    case "shop_id":
                        source.ShopID = (ShopID?)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "statpoints":
                        source.StatPoints = dataReader.GetInt32(i);
                        break;

                    case "str":
                        source.SetStat(StatType.Str, dataReader.GetByte(i));
                        break;

                    case "tact":
                        source.SetStat(StatType.Tact, dataReader.GetByte(i));
                        break;

                    case "ws":
                        source.SetStat(StatType.WS, dataReader.GetByte(i));
                        break;

                    case "x":
                        source.X = dataReader.GetFloat(i);
                        break;

                    case "y":
                        source.Y = dataReader.GetFloat(i);
                        break;
                }
            }
        }
    }
}