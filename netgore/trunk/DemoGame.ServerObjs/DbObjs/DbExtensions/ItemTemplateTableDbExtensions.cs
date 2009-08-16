using System;
using System.Data;
using System.Linq;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Contains extension methods for class ItemTemplateTable that assist in performing
    /// reads and writes to and from a database.
    /// </summary>
    public static class ItemTemplateTableDbExtensions
    {
        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(this IItemTemplateTable source, DbParameterValues paramValues)
        {
            paramValues["@agi"] = (Int16)source.GetStat(StatType.Agi);
            paramValues["@armor"] = (Int16)source.GetStat(StatType.Armor);
            paramValues["@bra"] = (Int16)source.GetStat(StatType.Bra);
            paramValues["@defence"] = (Int16)source.GetStat(StatType.Defence);
            paramValues["@description"] = source.Description;
            paramValues["@dex"] = (Int16)source.GetStat(StatType.Dex);
            paramValues["@evade"] = (Int16)source.GetStat(StatType.Evade);
            paramValues["@graphic"] = (UInt16)source.Graphic;
            paramValues["@height"] = source.Height;
            paramValues["@hp"] = (Int16)source.HP;
            paramValues["@id"] = (UInt16)source.ID;
            paramValues["@imm"] = (Int16)source.GetStat(StatType.Imm);
            paramValues["@int"] = (Int16)source.GetStat(StatType.Int);
            paramValues["@maxhit"] = (Int16)source.GetStat(StatType.MaxHit);
            paramValues["@maxhp"] = (Int16)source.GetStat(StatType.MaxHP);
            paramValues["@maxmp"] = (Int16)source.GetStat(StatType.MaxMP);
            paramValues["@minhit"] = (Int16)source.GetStat(StatType.MinHit);
            paramValues["@mp"] = (Int16)source.MP;
            paramValues["@name"] = source.Name;
            paramValues["@perc"] = (Int16)source.GetStat(StatType.Perc);
            paramValues["@reqacc"] = (Byte)source.GetReqStat(StatType.Acc);
            paramValues["@reqagi"] = (Byte)source.GetReqStat(StatType.Agi);
            paramValues["@reqarmor"] = (Byte)source.GetReqStat(StatType.Armor);
            paramValues["@reqbra"] = (Byte)source.GetReqStat(StatType.Bra);
            paramValues["@reqdex"] = (Byte)source.GetReqStat(StatType.Dex);
            paramValues["@reqevade"] = (Byte)source.GetReqStat(StatType.Evade);
            paramValues["@reqimm"] = (Byte)source.GetReqStat(StatType.Imm);
            paramValues["@reqint"] = (Byte)source.GetReqStat(StatType.Int);
            paramValues["@type"] = source.Type;
            paramValues["@value"] = source.Value;
            paramValues["@width"] = source.Width;
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="source">The object to add the extension method to.</param>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public static void ReadValues(this ItemTemplateTable source, IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("agi");
            source.SetStat(StatType.Agi, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("armor");
            source.SetStat(StatType.Armor, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("bra");
            source.SetStat(StatType.Bra, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("defence");
            source.SetStat(StatType.Defence, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("description");
            source.Description = dataReader.GetString(i);

            i = dataReader.GetOrdinal("dex");
            source.SetStat(StatType.Dex, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("evade");
            source.SetStat(StatType.Evade, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("graphic");
            source.Graphic = (GrhIndex)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("height");
            source.Height = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("hp");
            source.HP = dataReader.GetInt16(i);

            i = dataReader.GetOrdinal("id");
            source.ID = (ItemTemplateID)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("imm");
            source.SetStat(StatType.Imm, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("int");
            source.SetStat(StatType.Int, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("maxhit");
            source.SetStat(StatType.MaxHit, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("maxhp");
            source.SetStat(StatType.MaxHP, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("maxmp");
            source.SetStat(StatType.MaxMP, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("minhit");
            source.SetStat(StatType.MinHit, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("mp");
            source.MP = dataReader.GetInt16(i);

            i = dataReader.GetOrdinal("name");
            source.Name = dataReader.GetString(i);

            i = dataReader.GetOrdinal("perc");
            source.SetStat(StatType.Perc, dataReader.GetInt16(i));

            i = dataReader.GetOrdinal("reqacc");
            source.SetReqStat(StatType.Acc, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("reqagi");
            source.SetReqStat(StatType.Agi, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("reqarmor");
            source.SetReqStat(StatType.Armor, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("reqbra");
            source.SetReqStat(StatType.Bra, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("reqdex");
            source.SetReqStat(StatType.Dex, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("reqevade");
            source.SetReqStat(StatType.Evade, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("reqimm");
            source.SetReqStat(StatType.Imm, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("reqint");
            source.SetReqStat(StatType.Int, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("type");
            source.Type = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("value");
            source.Value = dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("width");
            source.Width = dataReader.GetByte(i);
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
        public static void TryCopyValues(this IItemTemplateTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@agi":
                        paramValues[i] = (Int16)source.GetStat(StatType.Agi);
                        break;

                    case "@armor":
                        paramValues[i] = (Int16)source.GetStat(StatType.Armor);
                        break;

                    case "@bra":
                        paramValues[i] = (Int16)source.GetStat(StatType.Bra);
                        break;

                    case "@defence":
                        paramValues[i] = (Int16)source.GetStat(StatType.Defence);
                        break;

                    case "@description":
                        paramValues[i] = source.Description;
                        break;

                    case "@dex":
                        paramValues[i] = (Int16)source.GetStat(StatType.Dex);
                        break;

                    case "@evade":
                        paramValues[i] = (Int16)source.GetStat(StatType.Evade);
                        break;

                    case "@graphic":
                        paramValues[i] = (UInt16)source.Graphic;
                        break;

                    case "@height":
                        paramValues[i] = source.Height;
                        break;

                    case "@hp":
                        paramValues[i] = (Int16)source.HP;
                        break;

                    case "@id":
                        paramValues[i] = (UInt16)source.ID;
                        break;

                    case "@imm":
                        paramValues[i] = (Int16)source.GetStat(StatType.Imm);
                        break;

                    case "@int":
                        paramValues[i] = (Int16)source.GetStat(StatType.Int);
                        break;

                    case "@maxhit":
                        paramValues[i] = (Int16)source.GetStat(StatType.MaxHit);
                        break;

                    case "@maxhp":
                        paramValues[i] = (Int16)source.GetStat(StatType.MaxHP);
                        break;

                    case "@maxmp":
                        paramValues[i] = (Int16)source.GetStat(StatType.MaxMP);
                        break;

                    case "@minhit":
                        paramValues[i] = (Int16)source.GetStat(StatType.MinHit);
                        break;

                    case "@mp":
                        paramValues[i] = (Int16)source.MP;
                        break;

                    case "@name":
                        paramValues[i] = source.Name;
                        break;

                    case "@perc":
                        paramValues[i] = (Int16)source.GetStat(StatType.Perc);
                        break;

                    case "@reqacc":
                        paramValues[i] = (Byte)source.GetReqStat(StatType.Acc);
                        break;

                    case "@reqagi":
                        paramValues[i] = (Byte)source.GetReqStat(StatType.Agi);
                        break;

                    case "@reqarmor":
                        paramValues[i] = (Byte)source.GetReqStat(StatType.Armor);
                        break;

                    case "@reqbra":
                        paramValues[i] = (Byte)source.GetReqStat(StatType.Bra);
                        break;

                    case "@reqdex":
                        paramValues[i] = (Byte)source.GetReqStat(StatType.Dex);
                        break;

                    case "@reqevade":
                        paramValues[i] = (Byte)source.GetReqStat(StatType.Evade);
                        break;

                    case "@reqimm":
                        paramValues[i] = (Byte)source.GetReqStat(StatType.Imm);
                        break;

                    case "@reqint":
                        paramValues[i] = (Byte)source.GetReqStat(StatType.Int);
                        break;

                    case "@type":
                        paramValues[i] = source.Type;
                        break;

                    case "@value":
                        paramValues[i] = source.Value;
                        break;

                    case "@width":
                        paramValues[i] = source.Width;
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
        public static void TryReadValues(this ItemTemplateTable source, IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "agi":
                        source.SetStat(StatType.Agi, dataReader.GetInt16(i));
                        break;

                    case "armor":
                        source.SetStat(StatType.Armor, dataReader.GetInt16(i));
                        break;

                    case "bra":
                        source.SetStat(StatType.Bra, dataReader.GetInt16(i));
                        break;

                    case "defence":
                        source.SetStat(StatType.Defence, dataReader.GetInt16(i));
                        break;

                    case "description":
                        source.Description = dataReader.GetString(i);
                        break;

                    case "dex":
                        source.SetStat(StatType.Dex, dataReader.GetInt16(i));
                        break;

                    case "evade":
                        source.SetStat(StatType.Evade, dataReader.GetInt16(i));
                        break;

                    case "graphic":
                        source.Graphic = (GrhIndex)dataReader.GetUInt16(i);
                        break;

                    case "height":
                        source.Height = dataReader.GetByte(i);
                        break;

                    case "hp":
                        source.HP = dataReader.GetInt16(i);
                        break;

                    case "id":
                        source.ID = (ItemTemplateID)dataReader.GetUInt16(i);
                        break;

                    case "imm":
                        source.SetStat(StatType.Imm, dataReader.GetInt16(i));
                        break;

                    case "int":
                        source.SetStat(StatType.Int, dataReader.GetInt16(i));
                        break;

                    case "maxhit":
                        source.SetStat(StatType.MaxHit, dataReader.GetInt16(i));
                        break;

                    case "maxhp":
                        source.SetStat(StatType.MaxHP, dataReader.GetInt16(i));
                        break;

                    case "maxmp":
                        source.SetStat(StatType.MaxMP, dataReader.GetInt16(i));
                        break;

                    case "minhit":
                        source.SetStat(StatType.MinHit, dataReader.GetInt16(i));
                        break;

                    case "mp":
                        source.MP = dataReader.GetInt16(i);
                        break;

                    case "name":
                        source.Name = dataReader.GetString(i);
                        break;

                    case "perc":
                        source.SetStat(StatType.Perc, dataReader.GetInt16(i));
                        break;

                    case "reqacc":
                        source.SetReqStat(StatType.Acc, dataReader.GetByte(i));
                        break;

                    case "reqagi":
                        source.SetReqStat(StatType.Agi, dataReader.GetByte(i));
                        break;

                    case "reqarmor":
                        source.SetReqStat(StatType.Armor, dataReader.GetByte(i));
                        break;

                    case "reqbra":
                        source.SetReqStat(StatType.Bra, dataReader.GetByte(i));
                        break;

                    case "reqdex":
                        source.SetReqStat(StatType.Dex, dataReader.GetByte(i));
                        break;

                    case "reqevade":
                        source.SetReqStat(StatType.Evade, dataReader.GetByte(i));
                        break;

                    case "reqimm":
                        source.SetReqStat(StatType.Imm, dataReader.GetByte(i));
                        break;

                    case "reqint":
                        source.SetReqStat(StatType.Int, dataReader.GetByte(i));
                        break;

                    case "type":
                        source.Type = dataReader.GetByte(i);
                        break;

                    case "value":
                        source.Value = dataReader.GetInt32(i);
                        break;

                    case "width":
                        source.Width = dataReader.GetByte(i);
                        break;
                }
            }
        }
    }
}